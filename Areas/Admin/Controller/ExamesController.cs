using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microlab.web.Data;
using Microlab.web.Models;
using Microlab.web.Models.Entities;
using X.PagedList;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Microlab.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ExamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Exame
        public async Task<IActionResult> Index(Guid? clinicaId, DateTime? dataInicial, DateTime? dataFinal, int page = 1, int pageSize = 10)
        {
            var vm = new AdminExamesViewModel
            {
                Clinicas = await _context.Clinicas.OrderBy(c => c.Nome).ToListAsync(),
                ClinicaId = clinicaId,
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                PageNumber = page,
                PageSize = pageSize
            };

            IQueryable<Exame> query = _context.Exames
                .Include(e => e.Paciente)
                .AsQueryable();

            if (clinicaId.HasValue)
                query = query.Where(e => e.Paciente.ClinicaId == clinicaId.Value);

            if (dataInicial.HasValue)
                query = query.Where(e => e.DataSolicitacao >= dataInicial.Value);

            if (dataFinal.HasValue)
                query = query.Where(e => e.DataSolicitacao <= dataFinal.Value);

            var examesList = await query.OrderByDescending(e => e.DataSolicitacao).ToListAsync();
            vm.Exames = examesList.ToPagedList(page, pageSize);

            return View(vm);
        }
    }
}
