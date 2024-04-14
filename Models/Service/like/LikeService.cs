using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.like
{
    public class LikeService : ILikeService
    {
        private readonly ClothingStoreDbContext _context;
        public LikeService(ClothingStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Like>> GetAllAsync()
        {
            return await _context.Likes.ToListAsync();
        }
    }
}
