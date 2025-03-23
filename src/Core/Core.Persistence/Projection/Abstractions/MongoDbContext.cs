using MongoDB.Driver;
using System.Security.Authentication;

namespace Core.Persistence.Projection.Abstractions
{
    public abstract class MongoDbContext : IMongoDbContext
    {
        public readonly IMongoClient _mongoClient;
        public readonly IMongoDatabase _database;

        protected MongoDbContext(string connectionString)
        {
            MongoUrl mongoUrl = new(connectionString);

            MongoClientSettings settings = MongoClientSettings.FromUrl(mongoUrl);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls13
            };

            _mongoClient = new MongoClient(settings);
            _database = _mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoClient MongoClient => _mongoClient;

        public string DatabaseName => _database.DatabaseNamespace.DatabaseName;

        public IMongoCollection<T> GetCollection<T>()
            => _database.GetCollection<T>(typeof(T).Name);
    }
}
