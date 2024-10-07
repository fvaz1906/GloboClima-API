namespace GloboClima.Domain.Models
{
    public class TokenModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Key { get; set; } = null!;
        public DateTime ValidTo { get; set; }
    }
}
