using Microlab.web.Data;
using Microlab.web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ExamesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExamesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Exames do paciente
    public async Task<IActionResult> Index(Guid pacienteId)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Exames)
            .Include(p => p.Clinica)
            .FirstOrDefaultAsync(p => p.PacienteId == pacienteId);

        if (paciente == null) return NotFound();

        return View(paciente);
    }

    // GET: Exames/Create
    public IActionResult Create(Guid pacienteId)
    {
        var exame = new Exame
        {
            PacienteId = pacienteId
        };
        return View(exame);
    }

    // POST: Exames/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Exame viewModel)
    {
        if(viewModel.Observacao == null) { viewModel.Observacao = "Nenhuma Observação"; }
        var exame = new Exame
        {
            NmExame = viewModel.NmExame,
            Material = viewModel.Material,
            Metodo = "Exame Físico-Químico / Microscopia",
            Digitador = "Sem digitador",
            Status = "Sem Resultado",
            Observacao = viewModel.Observacao,
            PacienteId = viewModel.PacienteId
        };
        
        if (!ModelState.IsValid)
        {
            exame.ExameId = Guid.NewGuid();

            _context.Exames.Add(exame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { pacienteId = exame.PacienteId });
        }
        return View(exame);
    }

    // GET: Exames/Edit
    public async Task<IActionResult> Edit(Guid id)
    {
        var exame = await _context.Exames.FindAsync(id);
        if (exame == null) return NotFound();
        return View(exame);
    }

    // POST: Exames/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Exame exame)
    {
        if (id != exame.ExameId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { pacienteId = exame.PacienteId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Exames.Any(e => e.ExameId == exame.ExameId))
                    return NotFound();
                throw;
            }
        }
        return View(exame);
    }

    // GET: Exames/Delete
    public async Task<IActionResult> Delete(Guid id)
    {
        var exame = await _context.Exames
            .Include(e => e.Paciente)
            .FirstOrDefaultAsync(e => e.ExameId == id);

        if (exame == null) return NotFound();

        return View(exame);
    }

    // POST: Exames/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var exame = await _context.Exames.FindAsync(id);
        if (exame != null)
        {
            var pacienteId = exame.PacienteId;
            _context.Exames.Remove(exame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { pacienteId });
        }
        return NotFound();
    }
}
