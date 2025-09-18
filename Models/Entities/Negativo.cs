namespace Microlab.web.Models.Entities
{
    public class Negativo
    {
        public Guid NegativoId { get; set; }
        public Guid ResultadoExameId { get; set; }
        public string Protozoarios { get; set; }
        public string Helmintos { get; set; }
        public string LarvasEncontradas { get; set; }
        public string Observacao { get; set; }

        public ResultadoExame ResultadoExame { get; set; }
    }
}
