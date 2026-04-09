using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Mappings;
using ProductApi.Models;
using ProductApi.Models.Dtos;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        List<Product> products = await _context.Products.ToListAsync();
        return products.Select(p => p.ToResponse()).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product?.ToResponse();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        Product product = request.ToEntity();

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.ToResponse();
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        Product? existing = await _context.Products.FindAsync(id);

        if (existing == null)
            return null;

        request.UpdateEntity(existing);
        await _context.SaveChangesAsync();
        return existing.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}