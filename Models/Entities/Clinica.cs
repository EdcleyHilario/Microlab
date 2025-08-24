namespace Microlab.web.Models.Entities
{
    public class Clinica
    {
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string UsuarioId { get; set; }
        public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
    }
}
