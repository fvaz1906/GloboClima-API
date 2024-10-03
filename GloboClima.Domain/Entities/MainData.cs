namespace GloboClima.Domain.Entities
{
    public class MainData : BaseEntity
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
    }
}
