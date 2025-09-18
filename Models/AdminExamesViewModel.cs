using Microlab.web.Models.Entities;
using X.PagedList;
using X.PagedList.Extensions;

namespace Microlab.web.Models
{
    public class AdminExamesViewModel
    {
        public Guid? ClinicaId { get; set; }  // filtro selecionado
        public List<Clinica> Clinicas { get; set; } = new();

        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        public IPagedList<Exame> Exames { get; set; } = new List<Exame>().ToPagedList(1, 10);

        // Paginação
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
