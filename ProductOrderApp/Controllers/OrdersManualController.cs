using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductOrderApp.Extensions;
using ProductOrderApp.Models.DTOs;
using ProductOrderApp.Models.Entities;
using ProductOrderApp.Repositories.Interfaces;

namespace ProductOrderApp.Controllers;

public class OrdersManualController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrdersManualController(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var orders = await _orderRepository.GetOrdersWithItemsAsync();
        var orderDtos = orders.Select(o => o.ToDto());
        return View(orderDtos);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _orderRepository.GetOrderWithItemsByIdAsync(id.Value);
        if (order == null)
        {
            return NotFound();
        }

        var orderDto = order.ToDto();
        return View(orderDto);
    }

    // GET: Orders/Create
    public async Task<IActionResult> Create()
    {
        var products = await _productRepository.GetProductsInStockAsync();
        ViewBag.Products = new SelectList(products, "Id", "Name");
        return View();
    }

    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderDto createOrderDto)
    {
        if (ModelState.IsValid && createOrderDto.OrderItems.Any())
        {
            var order = createOrderDto.ToEntity();

            // Calculate total amount
            decimal totalAmount = 0;
            foreach (var item in createOrderDto.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };
                    order.OrderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;
                }
            }

            order.TotalAmount = totalAmount;
            await _orderRepository.AddAsync(order);
            return RedirectToAction(nameof(Index));
        }

        var products = await _productRepository.GetProductsInStockAsync();
        ViewBag.Products = new SelectList(products, "Id", "Name");
        return View(createOrderDto);
    }

    // GET: Orders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _orderRepository.GetOrderWithItemsByIdAsync(id.Value);
        if (order == null)
        {
            return NotFound();
        }

        var products = await _productRepository.GetProductsInStockAsync();
        ViewBag.Products = new SelectList(products, "Id", "Name");

        var orderDto = order.ToDto();
        return View(orderDto);
    }

    // POST: Orders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, OrderDto orderDto)
    {
        if (id != orderDto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithItemsByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                // Update order properties
                order.CustomerName = orderDto.CustomerName;
                order.CustomerEmail = orderDto.CustomerEmail;
                order.Status = orderDto.Status;

                await _orderRepository.UpdateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                if (!await _orderRepository.ExistsAsync(orderDto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        var products = await _productRepository.GetProductsInStockAsync();
        ViewBag.Products = new SelectList(products, "Id", "Name");
        return View(orderDto);
    }

    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _orderRepository.GetOrderWithItemsByIdAsync(id.Value);
        if (order == null)
        {
            return NotFound();
        }

        var orderDto = order.ToDto();
        return View(orderDto);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order != null)
        {
            await _orderRepository.DeleteAsync(order);
        }

        return RedirectToAction(nameof(Index));
    }
}