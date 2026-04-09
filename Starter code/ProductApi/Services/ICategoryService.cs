using ProductApi.Models.Dtos;
using ProductApi.Common;

namespace ProductApi.Services
{
    public interface ICategoryService
    {
        Task<Result<List<CategoryResponse>>> GetAllAsync();
        Task<Result<CategoryResponse?>> GetByIdAsync(int id);
        Task<Result<CategoryResponse>> CreateAsync(CreateCategoryRequest request);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
