using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.category
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
    }
}
