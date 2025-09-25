using ProductOrderApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductOrderApp.Models.DTOs;

public class OrderDto
{
    public int Id { get; set; }

    [Display(Name = "Order Number")]
    public string OrderNumber { get; set; } = string.Empty;

    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Display(Name = "Customer Email")]
    public string? CustomerEmail { get; set; }

    [Display(Name = "Order Date")]
    public DateTime OrderDate { get; set; }

    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    [Display(Name = "Status")]
    public string StatusName => Status.ToString();

    public List<OrderItemDto> OrderItems { get; set; } = [];
}

public class CreateOrderDto
{
    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    [Display(Name = "Customer Email")]
    public string? CustomerEmail { get; set; }

    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CreateOrderItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}
