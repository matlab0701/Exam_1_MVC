using Domain.DTOs.Products;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

public class ProductController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] ProductFilter filter)
    {
        var response = await productService.GetAllAsync(filter);
        return View(response.Data);
        
    }


    public async Task<IActionResult> Details(int id)
    {
        var response = await productService.GetProductAsync(id);
        if (!response.IsSuccess) return NotFound(response.Message);
        return View(response.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var response = await productService.CreateAsync(dto);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await productService.GetProductAsync(id);
        if (!response.IsSuccess) return NotFound(response.Message);

        var product = response.Data;
        var updateDto = new UpdateProductDto
        {
            Name = product.Name,
            Price = product.Price
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateProductDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var response = await productService.UpDateAsync(id, dto);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await productService.GetProductAsync(id);
        if (!response.IsSuccess) return NotFound(response.Message);
        return View(response.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await productService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            TempData["Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

}
