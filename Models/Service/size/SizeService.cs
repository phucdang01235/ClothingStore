using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.size
{
    public class SizeService : ISizeService
    {
        private readonly ClothingStoreDbContext _context;
        public SizeService(ClothingStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Size>> GetAllAsync()
        {
            return await _context.Sizes.ToListAsync();
        }
    }
}
