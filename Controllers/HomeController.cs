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
        public async Task<IActionResult> Index(string search, string exame, string situacao, DateTime? inicio, DateTime? fim, int page = 1, int pageSize = 10)
        {
            // 1. Pega o usuário logado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            // 2. Busca a clínica associada ao usuário logado
            var clinica = await db.Clinicas
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            // 3. Se não existir, redireciona para cadastro de clínica
            if (clinica == null)
                return RedirectToAction("Create", "Clinicas");

            var clinicaId = clinica.ClinicaId;

            // 4. Consulta pacientes da clínica usando DbSet
            var query = db.Pacientes.Where(p => p.ClinicaId == clinicaId);

            // 5. Aplica filtros
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Nome.Contains(search) || p.Cpf.Contains(search));

            if (!string.IsNullOrEmpty(exame))
                query = query.Where(p => p.Exame == exame);

            if (inicio.HasValue)
                query = query.Where(p => p.DataSolicitacao >= inicio.Value);

            if (fim.HasValue)
                query = query.Where(p => p.DataSolicitacao <= fim.Value);

            // 🔹 Filtro por situação
            if (!string.IsNullOrEmpty(situacao))
            {
                if (situacao == "Liberado")
                    query = query.Where(p => p.Liberado);
                else if (situacao == "Solicitado")
                    query = query.Where(p => !p.Liberado);
            }

            // 6. Recupera todos os pacientes filtrados para o gráfico
            var pacientesParaGrafico = query
            .OrderBy(p => p.DataSolicitacao)
            .AsEnumerable() // avaliação em memória
            .ToList();


            // 7. Paginação para a tabela
            var totalCount = pacientesParaGrafico.Count;
            var pacientes = pacientesParaGrafico
                .OrderByDescending(p => p.DataSolicitacao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 8. Preparar dados do gráfico
            var datas = pacientesParaGrafico
                .Select(p => p.DataSolicitacao.ToString("dd/MM"))
                .Distinct()
                .ToList();

            var urinaPorData = datas.Select(d =>
                pacientesParaGrafico.Count(p => p.DataSolicitacao.ToString("dd/MM") == d && p.Exame == "SUMÁRIO DE URINA")
            ).ToList();

            var fezesPorData = datas.Select(d =>
                pacientesParaGrafico.Count(p => p.DataSolicitacao.ToString("dd/MM") == d && p.Exame == "PARACITOLÓGICO DE FEZES")
            ).ToList();

            var liberadosPorData = datas.Select(d =>
                pacientesParaGrafico.Count(p => p.DataSolicitacao.ToString("dd/MM") == d && p.Liberado)
            ).ToList();

            var solicitadosPorData = datas.Select(d =>
                pacientesParaGrafico.Count(p => p.DataSolicitacao.ToString("dd/MM") == d && !p.Liberado)
            ).ToList();

            // 9. Monta o ViewModel
            var viewModel = new HomeViewModel
            {
                ClinicaId = clinicaId,
                Pacientes = pacientes,
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
