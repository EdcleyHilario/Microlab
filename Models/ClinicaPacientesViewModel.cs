using Microlab.web.Models.Entities;
using X.PagedList;

namespace Microlab.web.Models
{
    public class PacienteItemViewModel
    {
        public Guid ClinicaId { get; set; }
        public Guid PacienteId { get; set; }
        public int Protocolo { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public int QtdExames { get; set; }
    }

    public class ClinicaPacientesViewModel
    {
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public Clinica Clinica { get; set; }
        public List<Clinica> Clinicas { get; set; } = new();
        public IPagedList<PacienteItemViewModel> Pacientes { get; set; }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
    }
}
