using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.product
{
    public class ProductService : IProductService
    {
        private readonly ClothingStoreDbContext _context;
        public  ProductService(ClothingStoreDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryName (string category)
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.Name.ToLower().Contains(category.ToLower()))
                .ToListAsync();
            return products;
        }


        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(i => i.ProductImages).FirstOrDefaultAsync(i => i.ProductId == id);
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Product product)
        {
            var productExisting = await _context.Products.FindAsync(id);
            productExisting.Name = product.Name;
            productExisting.Gender = product.Gender;
            productExisting.UnitPrice = product.UnitPrice;
            productExisting.Color = product.Color;
            productExisting.CategoryId = product.CategoryId;
            //  Bỏ qua ProductImage

            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            var productExisting = await _context.Products.FindAsync(id);
            _context.Products.Remove(productExisting);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> Search(string keyword)
        {
            return await _context.Products.Include(i => i.ProductImages).Where(i => i.Name.ToLower().Contains(keyword.ToLower())).ToListAsync();
        }
    }
}
