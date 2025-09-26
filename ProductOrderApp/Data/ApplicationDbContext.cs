using Microsoft.EntityFrameworkCore;
using ProductOrderApp.Models.Entities;
using ProductOrderApp.Models.DTOs;

namespace ProductOrderApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.StockQuantity).IsRequired();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedDate).IsRequired(false);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustomerEmail).HasMaxLength(200).IsRequired(false);
            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();
            // Remove database default for Status and handle in application code
            entity.Property(e => e.Status)
                .HasConversion<int>()
                .IsRequired();
        });

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Ignore(e => e.TotalPrice); // Computed property, not stored

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data - make sure CreatedDate is set
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "High-performance laptop",
                Price = 999.99m,
                StockQuantity = 50,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Description = "Wireless mouse",
                Price = 29.99m,
                StockQuantity = 100,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Name = "Keyboard",
                Description = "Mechanical keyboard",
                Price = 79.99m,
                StockQuantity = 75,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Configure all decimal properties to use precision 18,2 by default
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 2);
    }

//public DbSet<ProductOrderApp.Models.DTOs.ProductDto> ProductDto { get; set; } = default!;

//public DbSet<ProductOrderApp.Models.DTOs.OrderDto> OrderDto { get; set; } = default!;
}