using System.Collections.Generic;
using ProductApi.Mappings;
using ProductApi.Models;
using ProductApi.Models.Dtos;
using ProductApi.Repositories;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        List<Product> products = await _repository.GetAllAsync();
        return products.Select(p => p.ToResponse()).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        Product? product = await _repository.GetByIdAsync(id);
        return product?.ToResponse();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        Product product = request.ToEntity();
        Product created = await _repository.AddAsync(product);
        return created.ToResponse();
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        Product? existing = await _repository.GetByIdAsync(id);

        if (existing == null)
            return null;

        request.UpdateEntity(existing);
        await _repository.UpdateAsync(existing);
        return existing.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}