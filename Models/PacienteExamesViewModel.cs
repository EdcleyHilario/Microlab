using Microlab.web.Models.Entities;
using X.PagedList;
using System;
using System.Collections.Generic;

namespace Microlab.web.Models
{
    public class PacienteExamesViewModel
    {
        public Paciente Paciente { get; set; } = default!;
        public IPagedList<Exame> Exames { get; set; } = default!;

        // Filtros
        public string StatusFilter { get; set; } = string.Empty;
        public string TipoExameFilter { get; set; } = string.Empty;
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        // Dropdown de tipos de exame
        public List<string> TiposExamesDisponiveis { get; set; } = new();

        // Dropdown de status
        public List<string> StatusOpcoes { get; set; } = new() { "Liberado", "Solicitado" };

        // Paginação
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
