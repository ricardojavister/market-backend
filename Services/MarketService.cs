using MarketApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MarketApi.Services
{
    public class MarketService : IMarketService
    {
        private readonly IMongoCollection<Product> _products;

        public MarketService(IMongoDBSettings settings, MongoClient client)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _products = database.GetCollection<Product>(settings.ProductsCollectionName);
        }

        public async Task<List<Product>> GetProductsAsync(string keyword)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            keyword = keyword.ToLower();

            if (!string.IsNullOrEmpty(keyword))
            {
                string[] words = keyword.Split(" ");
                foreach (string word in words){
                    filter &= filterBuilder.Regex("name", new BsonRegularExpression(word, "i"));
                }
            }

            var sort = Builders<Product>.Sort.Ascending(p => p.Price);


            return await _products.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }
    }

    public interface IMarketService
    {
        Task<List<Product>> GetProductsAsync(string keyword);
        Task<List<Product>> GetAllProductsAsync();
    }
}
