using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.productsize
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly ClothingStoreDbContext _context;
        public ProductSizeService(ClothingStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductSize>> GetProductSizesByProductId(int productId)
        {
            return await _context.ProductSizes
                .Include(i => i.Size)
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }
    }
}
