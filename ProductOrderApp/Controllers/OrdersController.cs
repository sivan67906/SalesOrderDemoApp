using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductOrderApp.Models.DTOs;
using ProductOrderApp.Models.Entities;
using ProductOrderApp.Repositories.Interfaces;

namespace ProductOrderApp.Controllers;
public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var orders = await _orderRepository.GetOrdersWithItemsAsync();
        var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
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

        var orderDto = _mapper.Map<OrderDto>(order);
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
            var order = _mapper.Map<Order>(createOrderDto);

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

        var orderDto = _mapper.Map<OrderDto>(order);
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