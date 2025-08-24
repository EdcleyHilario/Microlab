using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microlab.web.Models.Entities
{
    public class Paciente
    {
        public Guid PacienteId { get; set; }

        [Required(ErrorMessage = "Nome do Paciente é Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Idade é Obrigatória")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Data de Nascimento é Obrigatória")]
        public DateTime DataNascimento { get; set; }

        public string Solicitante { get; set; }

        [Required(ErrorMessage = "Protocolo é Obrigatório")]
        public int Protocolo { get; set; }

        public string Rg { get; set; }
        public string Cpf { get; set; }

        [AllowedValues("SUMÁRIO DE URINA", "PARACITOLÓGICO DE FEZES", ErrorMessage = "Exame inválido.")]
        public string Exame { get; set; }

        [AllowedValues("URINA", "FEZES", ErrorMessage = "Material inválido.")]
        public string Material { get; set; }

        public DateTime DataSolicitacao { get; set; } = DateTime.Today;

        public bool Liberado { get; set; } = false;

        [Required]
        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; }
        public int QtdExames { get; set; }
        public ICollection<Exame> Exames { get; set; } = new List<Exame>();
    }
}
