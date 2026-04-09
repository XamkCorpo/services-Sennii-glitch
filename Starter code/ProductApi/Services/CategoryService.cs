using Microsoft.CodeAnalysis;
using ProductApi.Mappings;
using ProductApi.Models;
using ProductApi.Models.Dtos;
using ProductApi.Repositories;
using System.Collections.Generic;

namespace ProductApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            try
            {
                List<Category> categories = await _repository.GetAllAsync();
                return categories.Select(p => p.ToResponse()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Virhe kategorioiden haussa");
                throw;
            }
        }

        public async Task<CategoryResponse?> GetByIdAsync(int id)
        {
            try
            {
                Category? category = await _repository.GetByIdAsync(id);
                return category?.ToResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Virhe kategorian haussa: {CategoryId}", id);
                throw;
            }
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            try
            {
                Category category = request.ToEntity();

                Category created = await _repository.AddAsync(category);
                return created.ToResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Virhe kategorian luomisessa: {CategoryName}", request.Name);
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
                _logger.LogError(ex, "Virhe kategorian poistamisessa: {CategoryId}", id);
                throw;
            }
        }

    }

}
