using Microsoft.EntityFrameworkCore;
using ProductOrderApp.Data;
using ProductOrderApp.Models.Entities;
using ProductOrderApp.Repositories.Interfaces;

namespace ProductOrderApp.Repositories.Implementations;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsInStockAsync()
    {
        return await _dbSet.Where(p => p.StockQuantity > 0).ToListAsync();
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }
}