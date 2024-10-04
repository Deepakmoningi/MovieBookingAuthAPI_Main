using MongoDB.Driver;
using MovieBookingAuthApi.Interfaces;

namespace MovieBookingAuthApi.Models
{
    public class MongoDbContext
    {
        public readonly IMongoCollection<User> users;


        public MongoDbContext(IMongoDbConfig mongoConfig, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoConfig.DatabaseName);
            users = database.GetCollection<User>(mongoConfig.UsersCollectionName);
        }
    }

}
