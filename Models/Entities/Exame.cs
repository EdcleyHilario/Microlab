using System.ComponentModel.DataAnnotations;

namespace Microlab.web.Models.Entities
{
    public class Exame
    {
            public Guid ExameId { get; set; }

            [AllowedValues("SUMÁRIO DE URINA", "PARACITOLÓGICO DE FEZES", ErrorMessage = "Exame inválido.")]
            public string NmExame { get; set; }
            
            [AllowedValues("URINA", "FEZES", ErrorMessage = "Material inválido.")]
            public string Material { get; set; }
            public string Metodo { get; set; }
            public DateTime DataSolicitacao { get; set; }
            public DateTime DataDigitacao { get; set; }
            public string Digitador { get; set; }
            public bool Status { get; set; }
            public string Observacao { get; set; }
            public Guid PacienteId { get; set; }
            public Guid ClinicaId { get; set; }
            public Paciente Paciente { get; set; }

    }
}
