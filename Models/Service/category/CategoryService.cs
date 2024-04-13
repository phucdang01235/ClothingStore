using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.category
{
    public class CategoryService : ICategoryService
    {
        private readonly ClothingStoreDbContext _context;
        public CategoryService(ClothingStoreDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
