using Microsoft.Extensions.Options;
using MongoDB.Driver;
using scb_livecoding_productservice.Models;

namespace scb_livecoding_productservice.Service
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductService(IOptions<ProductDBSettings> productDBSettings)
        {
            var mongoClient = new MongoClient(productDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(productDBSettings.Value.DatabaseName);
            _productCollection = mongoDatabase.GetCollection<Product>(productDBSettings.Value.CollectionName);
        }
        public async Task<List<Product>> GetAsync() =>
            await _productCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product product) =>
            await _productCollection.InsertOneAsync(product);

        public async Task UpdateAsync(string id, Product product) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == id, product);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);
    }
}
