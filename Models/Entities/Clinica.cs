using System.ComponentModel.DataAnnotations;

namespace Microlab.web.Models.Entities
{
    public class Clinica
    {
        public Guid ClinicaId { get; set; }
        public required string UsuarioId { get; set; }
        [Required]
        public required string Nome { get; set; }
        public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
    }
}
