using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace GloboClima.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Photo { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
