using MovieBookingAuthApi.Interfaces;

namespace MovieBookingAuthApi.Models
{
    public class MongoDbConfig:IMongoDbConfig
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;
        
        public string UsersCollectionName { get; set; } = string.Empty;
    }
}
