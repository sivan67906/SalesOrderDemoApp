using Microsoft.AspNetCore.Mvc;
using ProductOrderApp.Extensions;
using ProductOrderApp.Models.DTOs;
using ProductOrderApp.Repositories.Interfaces;

namespace ProductOrderApp.Controllers;

public class ProductsManualController : Controller
{
    private readonly IProductRepository _productRepository;

    public ProductsManualController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();
        var productDtos = products.Select(p => p.ToDto());
        return View(productDtos);
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        var productDto = product.ToDto();
        return View(productDto);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Price,StockQuantity")] CreateProductDto createProductDto)
    {
        if (ModelState.IsValid)
        {
            var product = createProductDto.ToEntity();
            await _productRepository.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }
        return View(createProductDto);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        var productDto = product.ToDto();
        return View(productDto);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,StockQuantity,CreatedDate")] ProductDto productDto)
    {
        if (id != productDto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var product = productDto.ToEntity();
                product.UpdatedDate = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);
            }
            catch (Exception)
            {
                if (!await _productRepository.ExistsAsync(productDto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(productDto);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        var productDto = product.ToDto();
        return View(productDto);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            await _productRepository.DeleteAsync(product);
        }

        return RedirectToAction(nameof(Index));
    }
}