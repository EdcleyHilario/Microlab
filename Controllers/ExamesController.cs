using Microlab.web.Data;
using Microlab.web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QDocument = QuestPDF.Fluent.Document;

public class ExamesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExamesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Exames do paciente
    public async Task<IActionResult> Index(
    Guid pacienteId,
    string statusFilter,
    string tipoExameFilter,
    DateTime? dataInicial,
    DateTime? dataFinal,
    int page = 1,
    int pageSize = 10)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Clinica) 
            .Include(p => p.Exames)    
            .FirstOrDefaultAsync(p => p.PacienteId == pacienteId);

        if (paciente == null)
            return NotFound();

        var examesQuery = paciente.Exames.AsQueryable();

        // 🔹 Filtro por Status
        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (statusFilter == "Liberado")
                examesQuery = examesQuery.Where(e => e.Status == true);
            else if (statusFilter == "Solicitado")
                examesQuery = examesQuery.Where(e => e.Status == false);
        }

        // 🔹 Filtro por Tipo de Exame
        if (!string.IsNullOrEmpty(tipoExameFilter))
        {
            examesQuery = examesQuery.Where(e => e.NmExame == tipoExameFilter);
        }

        // 🔹 Filtro por Datas
        if (dataInicial.HasValue)
            examesQuery = examesQuery.Where(e => e.DataSolicitacao >= dataInicial.Value);

        if (dataFinal.HasValue)
            examesQuery = examesQuery.Where(e => e.DataSolicitacao <= dataFinal.Value);

        var examesPaged = examesQuery
            .OrderByDescending(e => e.DataSolicitacao)
            .ToPagedList(page, pageSize);

        var vm = new PacienteExamesViewModel
        {
            Paciente = paciente,
            Exames = examesPaged,
            StatusFilter = statusFilter,
            TipoExameFilter = tipoExameFilter,
            DataInicial = dataInicial,
            DataFinal = dataFinal,
            PageSize = pageSize,
            PageNumber = page,
            TiposExamesDisponiveis = paciente.Exames
                .Select(e => e.NmExame)
                .Distinct()
                .OrderBy(e => e)
                .ToList()
        };

        return View(vm);
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
            DataSolicitacao = DateTime.Now,
            Status = false,
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

    private IQueryable<Exame> FiltrarExames(Paciente paciente, string statusFilter, string tipoExameFilter, DateTime? dataInicial, DateTime? dataFinal)
    {
        var query = paciente.Exames.AsQueryable();

        if (!string.IsNullOrEmpty(statusFilter))
        {
            query = statusFilter == "Liberado" ? query.Where(e => e.Status) : query.Where(e => !e.Status);
        }

        if (!string.IsNullOrEmpty(tipoExameFilter))
        {
            query = query.Where(e => e.NmExame == tipoExameFilter);
        }

        if (dataInicial.HasValue)
            query = query.Where(e => e.DataSolicitacao >= dataInicial.Value);

        if (dataFinal.HasValue)
            query = query.Where(e => e.DataSolicitacao <= dataFinal.Value);

        return query.OrderByDescending(e => e.DataSolicitacao);
    }

    

public async Task<IActionResult> ExportarExcel(Guid pacienteId, string statusFilter, string tipoExameFilter, DateTime? dataInicial, DateTime? dataFinal)
{
    var paciente = await _context.Pacientes
        .Include(p => p.Exames)
        .FirstOrDefaultAsync(p => p.PacienteId == pacienteId);

    if (paciente == null)
        return NotFound();

    var exames = FiltrarExames(paciente, statusFilter, tipoExameFilter, dataInicial, dataFinal).ToList();

    using var workbook = new XLWorkbook();
    var worksheet = workbook.Worksheets.Add("Exames");

    // Cabeçalhos
    worksheet.Cell(1, 1).Value = "Exame";
    worksheet.Cell(1, 2).Value = "Método";
    worksheet.Cell(1, 3).Value = "Material";
    worksheet.Cell(1, 4).Value = "Status";
    worksheet.Cell(1, 5).Value = "Data Solicitação";

    // Dados
    for (int i = 0; i < exames.Count; i++)
    {
        var e = exames[i];
        worksheet.Cell(i + 2, 1).Value = e.NmExame;
        worksheet.Cell(i + 2, 2).Value = e.Metodo;
        worksheet.Cell(i + 2, 3).Value = e.Material;
        worksheet.Cell(i + 2, 4).Value = e.Status ? "Liberado" : "Solicitado";
        worksheet.Cell(i + 2, 5).Value = e.DataSolicitacao.ToString("dd/MM/yyyy HH:mm");
    }

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    stream.Position = 0;

    var fileName = $"Exames_{paciente.Nome}_{DateTime.Now:yyyyMMddHHmm}.xlsx";
    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
}
    public async Task<IActionResult> ExportarPdf(Guid pacienteId, string statusFilter, string tipoExameFilter, DateTime? dataInicial, DateTime? dataFinal)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Exames)
            .FirstOrDefaultAsync(p => p.PacienteId == pacienteId);

        if (paciente == null)
            return NotFound();

        var exames = FiltrarExames(paciente, statusFilter, tipoExameFilter, dataInicial, dataFinal).ToList();

        var pdf = QDocument.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Text($"Exames do Paciente: {paciente.Nome}").SemiBold().FontSize(16);
                page.Content().Table(table =>
                {
                    // Cabeçalhos
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn();
                        c.RelativeColumn();
                        c.RelativeColumn();
                        c.RelativeColumn();
                        c.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Exame");
                        header.Cell().Text("Método");
                        header.Cell().Text("Material");
                        header.Cell().Text("Status");
                        header.Cell().Text("Data Solicitação");
                    });

                    // Dados
                    foreach (var e in exames)
                    {
                        table.Cell().Text(e.NmExame);
                        table.Cell().Text(e.Metodo);
                        table.Cell().Text(e.Material);
                        table.Cell().Text(e.Status ? "Liberado" : "Solicitado");
                        table.Cell().Text(e.DataSolicitacao.ToString("dd/MM/yyyy HH:mm"));
                    }
                });
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);
        stream.Position = 0;

        var fileName = $"Exames_{paciente.Nome}_{DateTime.Now:yyyyMMddHHmm}.pdf";
        return File(stream.ToArray(), "application/pdf", fileName);
    }
}
