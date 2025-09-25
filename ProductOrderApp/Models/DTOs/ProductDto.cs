using System.ComponentModel.DataAnnotations;

namespace ProductOrderApp.Models.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Price ($)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Last Updated")]
    public DateTime? UpdatedDate { get; set; }
}

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Price ($)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }
}