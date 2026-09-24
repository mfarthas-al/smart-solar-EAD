/*
 * File: MongoDbService.cs
 * Author: Mohamed Farthas
 * Description: Wraps the MongoDB client/database so controllers never construct
 *              their own connection. Registered as a singleton in Program.cs and
 *              injected into other services/repositories that need a collection.
 */

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebService.Models;

namespace WebService.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        // Reads the connection string + database name from the bound MongoDbSettings
        // and opens the database handle once, for the lifetime of the app.
        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        // Generic helper so each controller/repository can fetch its own collection
        // by name (e.g. GetCollection<Station>("SolarStationInfo")) without repeating
        // client/database setup.
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
