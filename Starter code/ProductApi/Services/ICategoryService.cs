using ProductApi.Models.Dtos;

namespace ProductApi.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse?> GetByIdAsync(int id);
        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
