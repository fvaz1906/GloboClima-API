namespace GloboClima.Domain.Entities
{
    public class WeatherData : BaseEntity
    {
        public MainData Main { get; set; }
        public Wind Wind { get; set; }
        public string Name { get; set; }
    }
}
