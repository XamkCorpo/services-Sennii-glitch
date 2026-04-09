using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Mappings;
using ProductApi.Models;
using ProductApi.Models.Dtos;

namespace ProductApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            return categories.Select(p => p.ToResponse()).ToList();
        }

        public async Task<CategoryResponse?> GetByIdAsync(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            return category?.ToResponse();
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            Category category = request.ToEntity();

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.ToResponse();
        }

        public async Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            Category? existing = await _context.Categories.FindAsync(id);

            if (existing == null)
                return null;

            request.UpdateEntity(existing);
            await _context.SaveChangesAsync();
            return existing.ToResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
