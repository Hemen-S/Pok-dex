using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace PokedexAPI.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Pokemon> _pokemonCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _pokemonCollection = mongoDatabase.GetCollection<Pokemon>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Pokemon>> GetAsync() =>
            await _pokemonCollection.Find(_ => true).ToListAsync();

        public async Task<Pokemon?> GetAsync(string id) =>
            await _pokemonCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Pokemon newPokemon) =>
            await _pokemonCollection.InsertOneAsync(newPokemon);

        public async Task UpdateAsync(string id, Pokemon updatedPokemon) =>
            await _pokemonCollection.ReplaceOneAsync(x => x.Id == id, updatedPokemon);

        public async Task RemoveAsync(string id) =>
            await _pokemonCollection.DeleteOneAsync(x => x.Id == id);
    }

    public class Pokemon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        [BsonElement("Type")]
        public string Type { get; set; } = null!;

        [BsonElement("Level")]
        public int Level { get; set; }
    }
}
