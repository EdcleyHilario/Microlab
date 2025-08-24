using Microlab.web.Models.Entities;

namespace Microlab.web.Models
{
    public class AddExameViewModel
    {
        public Guid ExameId { get; set; }
        public string NmExame { get; set; }
        public string Metodo { get; set; }
        public string Material { get; set; }
        public DateTime DataDigitacao { get; set; }
        public string Digitador { get; set; }
        public string Status { get; set; }
        public string Observacao { get; set; }
        public string PacienteId { get; set; }

        public List<Exame> Exames { get; set; } = new();
    }
}
