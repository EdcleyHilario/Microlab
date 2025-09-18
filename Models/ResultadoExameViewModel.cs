using Microlab.web.Models.Entities;

namespace Microlab.web.Models.ViewModels
{
    public class ResultadoExameViewModel
    {
        public Guid ExameId { get; set; }
        public string NmExame { get; set; }
        public string PacienteNome { get; set; }

        public string StatusResultado { get; set; } // Negativo / Positivo / Inconclusivo
        public string ObservacaoGeral { get; set; }

        // Subtabelas (serão preenchidas se o exame for positivo ou negativo)
        public CaractereFisico CaracteresFisicos { get; set; } = new();
        public AnaliseQuimica AnaliseQuimica { get; set; } = new();
        public Sedimentoscopia Sedimentoscopia { get; set; } = new();
        public Negativo Negativo { get; set; } = new();
    }
}
