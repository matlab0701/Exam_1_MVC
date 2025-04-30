using Domain.DTOs.Customers;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

public class CustomerController(ICustomerService customerService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] CustomerFilter filter)
    {
        var response = await customerService.GetAllAsync(filter);
        return View(response.Data);
    }

    public async Task<IActionResult> Details(int Id)
    {
        var response = await customerService.GetCustomerAsync(Id);
        if (response.IsSuccess == false)
        {
            return NotFound(response.Message);
        }
        return View(response.Data);
    }


    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        if (ModelState.IsValid == false)
        {
            return View(dto);
        }
        var response = await customerService.CreateAsync(dto);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(dto);
        }
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int Id)
    {
        var response = await customerService.GetCustomerAsync(Id);
        if (!response.IsSuccess)
        {
            return NotFound(response.Message);
        }
        var customer = response.Data;
        var updateDto = new UpdateCustomerDto
        {
            FullName = customer.FullName,
            Email = customer.Email,

        };
        return View(updateDto);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int Id, UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var response = await customerService.UpDateAsync(Id, dto);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await customerService.GetCustomerAsync(id);
        if (!response.IsSuccess) return NotFound(response.Message);
        return View(response.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await customerService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            TempData["Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }


}
