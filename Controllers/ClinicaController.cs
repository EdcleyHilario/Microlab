using Microlab.web.Data;
using Microlab.web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microlab.web.Controllers
{
    [Authorize] // Garante que só usuários logados podem acessar
    public class ClinicasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClinicasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lista clínicas do usuário logado
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var clinicas = await _context.Clinicas
                .Where(c => c.UsuarioId == user.Id)
                .ToListAsync();

            return View(clinicas);
        }

        // GET: Clinicas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clinicas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome")] Clinica clinicaForm)
        {
            if (string.IsNullOrWhiteSpace(clinicaForm.Nome))
            {
                ModelState.AddModelError("Nome", "O nome da clínica é obrigatório");
                return View(clinicaForm);
            }

            var user = await _userManager.GetUserAsync(User);

            var clinica = new Clinica
            {
                ClinicaId = Guid.NewGuid(),
                Nome = clinicaForm.Nome,
                UsuarioId = user.Id,
                Pacientes = new List<Paciente>() // inicializa vazia
            };

            _context.Add(clinica);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Clinicas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var clinica = await _context.Clinicas.FindAsync(id);
            if (clinica == null) return NotFound();

            return View(clinica);
        }

        // POST: Clinicas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ClinicaId,Nome")] Clinica clinica)
        {
            if (id != clinica.ClinicaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    clinica.UsuarioId = user.Id; // mantém vínculo com o usuário logado
                    _context.Update(clinica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicaExists(clinica.ClinicaId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clinica);
        }

        // GET: Clinicas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var clinica = await _context.Clinicas
                .FirstOrDefaultAsync(m => m.ClinicaId == id);

            if (clinica == null) return NotFound();

            return View(clinica);
        }

        // POST: Clinicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var clinica = await _context.Clinicas.FindAsync(id);
            if (clinica != null)
            {
                _context.Clinicas.Remove(clinica);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicaExists(Guid id)
        {
            return _context.Clinicas.Any(e => e.ClinicaId == id);
        }
    }
}
