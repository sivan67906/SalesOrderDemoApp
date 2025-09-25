using ProductOrderApp.Models.Entities;

namespace ProductOrderApp.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsInStockAsync();
    Task<Product?> GetProductByNameAsync(string name);
}