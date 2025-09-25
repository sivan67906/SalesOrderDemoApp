using Microsoft.EntityFrameworkCore;
using ProductOrderApp.Data;
using ProductOrderApp.Models.Entities;
using ProductOrderApp.Repositories.Interfaces;

namespace ProductOrderApp.Repositories.Implementations;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersWithItemsAsync()
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithItemsByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerName)
    {
        return await _dbSet
            .Where(o => o.CustomerName.Contains(customerName))
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }
}