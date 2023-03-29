using MongoDB.Driver;

namespace Anni.Modules.Database
{
    public class DatabaseController
    {
        public static MongoClient _client { get; set; } = new MongoClient(Program._config.connectionString);
        public static IMongoDatabase _database { get; set; } = new MongoClient(Program._config.connectionString).GetDatabase(Program._config.databaseName);
        public static async Task Init()
        {
            _client = new MongoClient(Program._config.connectionString ?? "mongodb://192.168.20.34:27017/");
            _database = _client.GetDatabase(Program._config.databaseName);
        }
    }
}
