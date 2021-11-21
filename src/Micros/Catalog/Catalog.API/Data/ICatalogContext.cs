using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }

    internal class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration cfg)
        {
            IMongoClient client = new MongoClient(connectionString: cfg.GetSection("DatabaseSettings:ConnectionString").Value);
            var database = client.GetDatabase(cfg.GetSection("DatabaseSettings:DatabaseName").Value);

            Products = database.GetCollection<Product>(cfg.GetSection("DatabaseSettings:CollectionName").Value);
            
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}