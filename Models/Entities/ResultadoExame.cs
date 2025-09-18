
namespace Microlab.web.Models.Entities
{
    public class ResultadoExame
    {
        public Guid ResultadoExameId { get; set; }
        public Guid ExameId { get; set; }

        public string StatusResultado { get; set; } // Negativo / Positivo / Inconclusivo
        public string ObservacaoGeral { get; set; }
        public DateTime DataResultado { get; set; }
        public Exame Exame { get; set; }
        public CaractereFisico CaracteresFisicos { get; set; }
        public AnaliseQuimica AnaliseQuimica { get; set; }
        public Sedimentoscopia Sedimentoscopia { get; set; }
        public Negativo Negativo { get; set; }
    }
}