using Amazon.DynamoDBv2.DataModel;

namespace GloboClima.Domain.Entities
{
    [DynamoDBTable("Favorite")]
    public class Favorite
    {
        [DynamoDBHashKey]
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

    }
}
