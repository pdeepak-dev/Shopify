using System;
using MongoDB.Driver;
using Catalog.API.Data;
using Catalog.API.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName);

        Task CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);

    }

    internal class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<IEnumerable<Product>> GetProductsAsync()
            => await _context.Products.Find(p => true).ToListAsync();

        public async Task<Product> GetProductAsync(string id)
            => await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
            => await _context.Products.Find(Builders<Product>.Filter.Eq(p => p.Name, name)).ToListAsync();

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName)
            => await _context.Products.Find(Builders<Product>.Filter.Eq(p => p.Category, categoryName)).ToListAsync();

        public async Task CreateProductAsync(Product product)
            => await _context.Products.InsertOneAsync(product);

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updatedResult = await _context.Products.ReplaceOneAsync(Builders<Product>.Filter.Eq(p => p.Id, product.Id), product);
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var deletedResult = await _context.Products.DeleteOneAsync(Builders<Product>.Filter.Eq(p => p.Id, id));
            return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
        }
    }
}