using ProductOrderApp.Models.DTOs;
using ProductOrderApp.Models.Entities;

namespace ProductOrderApp.Extensions;

public static class MappingExtensions
{
    // Product mappings
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedDate = product.CreatedDate,
            UpdatedDate = product.UpdatedDate
        };
    }

    public static Product ToEntity(this ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity,
            CreatedDate = productDto.CreatedDate,
            UpdatedDate = productDto.UpdatedDate
        };
    }

    public static Product ToEntity(this CreateProductDto createProductDto)
    {
        return new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            StockQuantity = createProductDto.StockQuantity,
            CreatedDate = DateTime.UtcNow
        };
    }

    // Order mappings
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            OrderItems = order.OrderItems?.Select(oi => oi.ToDto()).ToList() ?? []
        };
    }

    public static Order ToEntity(this CreateOrderDto createOrderDto)
    {
        return new Order
        {
            OrderNumber = GenerateOrderNumber(),
            CustomerName = createOrderDto.CustomerName,
            CustomerEmail = createOrderDto.CustomerEmail,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            OrderItems = []
        };
    }

    // OrderItem mappings
    public static OrderItemDto ToDto(this OrderItem orderItem)
    {
        return new OrderItemDto
        {
            Id = orderItem.Id,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product?.Name ?? string.Empty,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.Quantity * orderItem.UnitPrice
        };
    }

    //public static OrderItem ToEntity(this CreateOrderItemDto createOrderItemDto)
    //{
    //    return new OrderItem
    //    {
    //        ProductId = createOrderItemDto.ProductId,
    //        Quantity = createOrderItemDto.Quantity
    //    };
    //}

    // Helper method
    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}