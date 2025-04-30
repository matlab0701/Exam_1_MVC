using Domain.DTOs.OrderItems;
using Domain.DTOs.Orders;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

public class OrderItemController(IOrderItemService orderItemService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] OrderItemFilter filter)
    {
        var response = await orderItemService.GetAllAsync(filter);
        return View(response.Data);
    }

    public async Task<IActionResult> Details(int Id)
    {
        var response = await orderItemService.GetOrderItemAsync(Id);
        if (response.IsSuccess == false)
        {
            return NotFound(response.Message);
        }
        return View(response.Data);
    }


    public async Task<IActionResult> Create()
    { return View(); }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderItemDto dto)
    {
        if (ModelState.IsValid == false)
        {
            return View(dto);
        }
        var response = await orderItemService.CreateAsync(dto);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(dto);
        }
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int Id)
    {
        var response = await orderItemService.GetOrderItemAsync(Id);
        if (!response.IsSuccess)
        {
            return NotFound(response.Message);
        }
        var customer = response.Data;
        var updateDto = new UpdateOrderItemDto
        {
            OrderId = customer.OrderId,
            ProductId = customer.ProductId,
            Quantity = customer.Quantity

        };
        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int Id, UpdateOrderItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var response = await orderItemService.UpDateAsync(Id, dto);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await orderItemService.GetOrderItemAsync(id);
        if (!response.IsSuccess) return NotFound(response.Message);
        return View(response.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await orderItemService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            TempData["Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}
