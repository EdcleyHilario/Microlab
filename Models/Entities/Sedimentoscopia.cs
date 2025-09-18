namespace Microlab.web.Models.Entities
{
    public class Sedimentoscopia
    {
        public Guid SedimentoscopiaId { get; set; }
        public Guid ResultadoExameId { get; set; }

        public string CelulasEpiteliais { get; set; }
        public string Leucocitos { get; set; }
        public string Hemacias { get; set; }
        public string Cristais { get; set; }
        public string Bacterias { get; set; }
        public string Cilindros { get; set; }
        public string Obs_Sedimentoscopia { get; set; }

        public ResultadoExame ResultadoExame { get; set; }
    }
}
