using ProductOrderApp.Models.Entities;

namespace ProductOrderApp.Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersWithItemsAsync();
    Task<Order?> GetOrderWithItemsByIdAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerName);
}