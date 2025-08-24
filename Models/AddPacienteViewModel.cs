using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microlab.web.Models.Entities;

namespace Microlab.web.Models
{
    public class AddPacienteViewModel
    {
        public Guid ClinicaId { get; set; }
        public string ClinicaNome { get; set; }

        [Required(ErrorMessage = "Nome do Paciente é Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Idade é Obrigatória")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Data de Nascimento é Obrigatória")]
        [DataType(DataType.Date)]
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

        // Opções para Select
        public List<string> ExamesDisponiveis { get; set; } = new() { "SUMÁRIO DE URINA", "PARACITOLÓGICO DE FEZES" };
        public List<string> MateriaisDisponiveis { get; set; } = new() { "URINA", "FEZES" };

        public DateTime DataSolicitacao { get; set; } = DateTime.Today;
        public bool Liberado { get; set; }
        public int QtdExames { get; set; }
        public List<Paciente> Pacientes { get; set; } = new();
    }
}
