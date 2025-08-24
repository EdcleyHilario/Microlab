using System;
using System.Collections.Generic;
using Microlab.web.Models.Entities;

namespace Microlab.web.Models
{
    public class HomeViewModel
    {

        public Guid ClinicaId { get; set; }

        // Filtros
        public string Exame { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }

        // Lista de pacientes
        public List<Paciente> Pacientes { get; set; } = new();

        // Dados do gráfico
        public List<string> Labels { get; set; } = new();
        public List<int> UrinaPorData { get; set; } = new();
        public List<int> FezesPorData { get; set; } = new();
        public string Situacao { get; set; }
        // Paginação
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        // MiniGrafico
        public List<int> LiberadosPorData { get; set; } = new List<int>();
        public List<int> SolicitadosPorData { get; set; } = new List<int>();



    }
}
