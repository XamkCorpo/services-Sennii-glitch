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

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            List<Category> categories = await _repository.GetAllAsync();
            return categories.Select(p => p.ToResponse()).ToList();
        }

        public async Task<CategoryResponse?> GetByIdAsync(int id)
        {
            Category? category = await _repository.GetByIdAsync(id);
            return category?.ToResponse();
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            Category category = request.ToEntity();

            Category created = await _repository.AddAsync(category);
            return created.ToResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

    }

}
