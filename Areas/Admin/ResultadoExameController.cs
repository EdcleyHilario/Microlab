using Microlab.web.Data;
using Microlab.web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microlab.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResultadoExameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultadoExameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ResultadoExame/Create?exameId=xxx
        public async Task<IActionResult> Create(Guid exameId)
        {
            var exame = await _context.Exames
                .Include(e => e.Paciente)
                .FirstOrDefaultAsync(e => e.ExameId == exameId);

            if (exame == null) return NotFound();

            var resultado = new ResultadoExame
            {
                ExameId = exameId,
                DataResultado = DateTime.Now
            };

            ViewBag.Exame = exame;
            return View(resultado);
        }

        // POST: Admin/ResultadoExame/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultadoExame resultado)
        {
            if (!ModelState.IsValid)
            {
                var exame = await _context.Exames
                    .Include(e => e.Paciente)
                    .FirstOrDefaultAsync(e => e.ExameId == resultado.ExameId);
                ViewBag.Exame = exame;
                return View(resultado);
            }

            resultado.ResultadoExameId = Guid.NewGuid();
            resultado.DataResultado = DateTime.Now;

            _context.ResultadoExames.Add(resultado);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Exames", new { area = "Admin", clinicaId = resultado.Exame.ClinicaId });
        }
    }
}
