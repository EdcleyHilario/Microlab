using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microlab.web.Data;
using Microlab.web.Models.Entities;
using Microlab.web.Models.ViewModels;

namespace Microlab.web.Controllers
{
    public class ResultadoExameController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ResultadoExameController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Criar resultado
        public async Task<IActionResult> Create(Guid exameId)
        {
            var exame = await _db.Exames
                .Include(e => e.Paciente)
                .FirstOrDefaultAsync(e => e.ExameId == exameId);

            if (exame == null) return NotFound();

            var vm = new ResultadoExameViewModel
            {
                ExameId = exame.ExameId,
                NmExame = exame.NmExame,
                PacienteNome = exame.Paciente.Nome
            };

            return View(vm);
        }

        // POST: Salvar resultado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultadoExameViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var resultado = new ResultadoExame
            {
                ResultadoExameId = Guid.NewGuid(),
                ExameId = vm.ExameId,
                StatusResultado = vm.StatusResultado,
                ObservacaoGeral = vm.ObservacaoGeral,
                DataResultado = DateTime.Now,
                CaracteresFisicos = vm.StatusResultado == "Positivo" ? vm.CaracteresFisicos : null,
                AnaliseQuimica = vm.StatusResultado == "Positivo" ? vm.AnaliseQuimica : null,
                Sedimentoscopia = vm.StatusResultado == "Positivo" ? vm.Sedimentoscopia : null,
                Negativo = vm.StatusResultado == "Negativo" ? vm.Negativo : null
            };

            _db.ResultadoExames.Add(resultado);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Exames", new { id = vm.ExameId });
        }

        // GET: Detalhes do resultado
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var resultado = await _db.ResultadoExames
                .Include(r => r.Exame)
                    .ThenInclude(e => e.Paciente)
                        .ThenInclude(p => p.Clinica)
                .Include(r => r.CaracteresFisicos)
                .Include(r => r.AnaliseQuimica)
                .Include(r => r.Sedimentoscopia)
                .Include(r => r.Negativo)
                .FirstOrDefaultAsync(r => r.ResultadoExameId == id);

           if (resultado == null) return NotFound();

            return View(resultado);
        }

    }
}
