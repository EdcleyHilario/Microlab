using System.Diagnostics;
using System.Security.Claims;
using Microlab.web.Data;
using Microlab.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microlab.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            db = context;
        }
        [Authorize]
        public async Task<IActionResult> Index( string search, string exame, string situacao, DateTime? inicio, DateTime? fim, int page = 1, int pageSize = 10)
        {
            // 1. Pega o usuário logado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            // 2. Busca a clínica associada ao usuário logado
            var clinica = await db.Clinicas
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (clinica == null)
                return RedirectToAction("Create", "Clinicas");

            var clinicaId = clinica.ClinicaId;

            // 3. Consulta exames da clínica
            var query = db.Exames
                .Include(e => e.Paciente)
                .Where(e => e.ClinicaId == clinicaId);

            // 4. Aplica filtros
            if (!string.IsNullOrEmpty(search))
                query = query.Where(e => e.Paciente.Nome.Contains(search) || e.Paciente.Cpf.Contains(search));

            if (!string.IsNullOrEmpty(exame))
                query = query.Where(e => e.NmExame == exame);

            if (inicio.HasValue)
                query = query.Where(e => e.DataDigitacao >= inicio.Value);

            if (fim.HasValue)
                query = query.Where(e => e.DataDigitacao <= fim.Value);

            if (!string.IsNullOrEmpty(situacao))
            {
                if (situacao == "Liberado")
                    query = query.Where(e => e.Status);
                else if (situacao == "Solicitado")
                    query = query.Where(e => !e.Status);
            }

            // 5. Recupera pacientes distintos diretamente no banco
            var pacientesQuery = query
                .Select(e => e.Paciente)
                .Distinct()
                .OrderByDescending(p => p.DataSolicitacao);

            var totalCount = await pacientesQuery.CountAsync();

            var pacientesPagina = await pacientesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 6. Preparar dados do gráfico (exames filtrados)
            var examesFiltrados = await query
                .OrderBy(e => e.Paciente.DataSolicitacao)
                .ToListAsync();

            var datas = examesFiltrados
                .Select(e => e.Paciente.DataSolicitacao.ToString("dd/MM"))
                .Distinct()
                .ToList();

            var urinaPorData = datas.Select(d =>
                examesFiltrados.Count(e => e.Paciente.DataSolicitacao.ToString("dd/MM") == d && e.NmExame == "SUMÁRIO DE URINA")
            ).ToList();

            var fezesPorData = datas.Select(d =>
                examesFiltrados.Count(e => e.Paciente.DataSolicitacao.ToString("dd/MM") == d && e.NmExame == "PARACITOLÓGICO DE FEZES")
            ).ToList();

            var liberadosPorData = datas.Select(d =>
                examesFiltrados.Count(e => e.Paciente.DataSolicitacao.ToString("dd/MM") == d && e.Status)
            ).ToList();

            var solicitadosPorData = datas.Select(d =>
                examesFiltrados.Count(e => e.Paciente.DataSolicitacao.ToString("dd/MM") == d && !e.Status)
            ).ToList();

            // 7. Monta o ViewModel
            var viewModel = new HomeViewModel
            {
                ClinicaId = clinicaId,
                Pacientes = pacientesPagina, // List<Paciente>
                Exame = exame,
                Inicio = inicio,
                Fim = fim,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Labels = datas,
                UrinaPorData = urinaPorData,
                FezesPorData = fezesPorData,
                LiberadosPorData = liberadosPorData,
                SolicitadosPorData = solicitadosPorData
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
