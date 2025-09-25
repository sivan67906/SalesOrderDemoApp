using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApp.Models.DTOs;
using ProductOrderApp.Models.Entities;
using ProductOrderApp.Repositories.Interfaces;

namespace ProductOrderApp.Controllers;
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
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

        var productDto = _mapper.Map<ProductDto>(product);
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
            var product = _mapper.Map<Product>(createProductDto);
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

        var productDto = _mapper.Map<ProductDto>(product);
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
                var product = _mapper.Map<Product>(productDto);
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

        var productDto = _mapper.Map<ProductDto>(product);
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