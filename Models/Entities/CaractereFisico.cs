namespace Microlab.web.Models.Entities
{
    public class CaractereFisico
    {
        public Guid CaractereFisicoId { get; set; }
        public Guid ResultadoExameId { get; set; }
        public string Volume { get; set; }
        public string Cor { get; set; }
        public string Aspecto { get; set; }
        public string Densidade { get; set; }
        public string PH { get; set; }
        public string Odor { get; set; }
        public string Obs_Caraterfisico { get; set; }

        public ResultadoExame ResultadoExame { get; set; }
    }
}
