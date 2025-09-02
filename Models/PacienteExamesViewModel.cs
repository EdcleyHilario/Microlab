using Microlab.web.Models.Entities;
using X.PagedList;
using System;

public class PacienteExamesViewModel
{
    public Paciente Paciente { get; set; }
    public IPagedList<Exame> Exames { get; set; }

    // Filtros
    public string StatusFilter { get; set; }
    public string TipoExameFilter { get; set; }
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }

    // Dropdown de tipos de exame
    public List<string> TiposExamesDisponiveis { get; set; } = new();

    // Paginação
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}
