using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ProductApi.Mappings;
using ProductApi.Models;
using ProductApi.Models.Dtos;
using ProductApi.Repositories;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        try
        {
            List<Product> products = await _repository.GetAllAsync();
            return products.Select(p => p.ToResponse()).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Virhe tuotteiden haussa");
            throw;
        }
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        try
        {
            Product? product = await _repository.GetByIdAsync(id);
            return product?.ToResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Virhe tuotteen haussa: {ProductId}", id);
            throw;
        }
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        try
        {
            Product product = request.ToEntity();
            Product created = await _repository.AddAsync(product);
            return created.ToResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Virhe tuotteen luomisessa: {ProductName}", request.Name);
            throw;
        }
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        try
        {
            Product? existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return null;

            request.UpdateEntity(existing);
            await _repository.UpdateAsync(existing);
            return existing.ToResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Virhe tuotteen päivittämisessä: {ProductId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            return await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Virhe tuotteen poistamisessa: {ProductId}", id);
            throw;
        }
    }
}