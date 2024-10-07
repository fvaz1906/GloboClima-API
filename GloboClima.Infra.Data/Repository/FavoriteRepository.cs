using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;

namespace GloboClima.Infra.Data.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public FavoriteRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<List<Favorite>> GetFavoritesByUserAsync(string userId)
        {
            var context = new DynamoDBContext(_dynamoDbClient);
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("UserId", ScanOperator.Equal, userId)
            };

            return await context.ScanAsync<Favorite>(conditions).GetRemainingAsync();
        }

        public async Task SaveFavoriteAsync(Favorite favorite)
        {
            var context = new DynamoDBContext(_dynamoDbClient);
            await context.SaveAsync(favorite);
        }

        public async Task DeleteFavoriteAsync(string userId, string locationId)
        {
            var context = new DynamoDBContext(_dynamoDbClient);
            await context.DeleteAsync<Favorite>(locationId, userId);
        }
    }
}
