using System.ComponentModel.DataAnnotations;

namespace ProductOrderApp.Models.Entities;

public class Order
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(200)]
    public string? CustomerEmail { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    // Navigation property
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}

public enum OrderStatus
{
    Pending = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}