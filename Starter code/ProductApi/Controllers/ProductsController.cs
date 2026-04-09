using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.Dtos;
using ProductApi.Services;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<ProductResponse> products = await _service.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        ProductResponse? product = await _service.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        ProductResponse created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request)
    {
        ProductResponse? updated = await _service.UpdateAsync(id, request);

        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}