using Microlab.web.Models.Entities;

namespace Microlab.web.Models
{
    public class ExameFilterViewModel
    {
        public Guid? ClinicaId { get; set; }
        public List<Clinica> Clinicas { get; set; } = new List<Clinica>();
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public List<Exame> Exames { get; set; } = new List<Exame>();
    }
}
