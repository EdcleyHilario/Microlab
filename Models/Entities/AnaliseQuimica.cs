namespace Microlab.web.Models.Entities
{
    public class AnaliseQuimica
    {
        public Guid AnaliseQuimicaId { get; set; }
        public Guid ResultadoExameId { get; set; }

        public string Glicose { get; set; }
        public string Proteinas { get; set; }
        public string Urobilinogenio { get; set; }
        public string Nitrito { get; set; }
        public string CorposCetonicos { get; set; }
        public string Leococitos { get; set; }
        public string Sangue { get; set; }
        public string Bilirrubina { get; set; }
        public string AcidoAscorbico { get; set; }
        public string Hemoglobina { get; set; }
        public string Obs_AnaliseQuimica { get; set; }

        public ResultadoExame ResultadoExame { get; set; }
    }
}
