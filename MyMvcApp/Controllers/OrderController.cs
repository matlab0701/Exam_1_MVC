using Domain.DTOs.Orders;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

public class OrderController(IOrderService orderService) : Controller
{
    public async Task<IActionResult> Index(OrderFilter filter)
    {
        var result = await orderService.GetAllAsync(filter);
        return View(result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await orderService.GetOrderAsync(id);
        if (result.Data == null)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto request)
    {
        var result = await orderService.CreateAsync(request);
        if (!result.IsSuccess)
        {
        ModelState.AddModelError(string.Empty, result.Message);
        return View(request);
        }
            return RedirectToAction(nameof(Index));
    }

}
