namespace MovieBookingAuthApi.Interfaces
{
    public interface IMongoDbConfig
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string UsersCollectionName { get; set; }



    }
}
