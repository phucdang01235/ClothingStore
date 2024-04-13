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
                .Include(p => p.ProductSizes)
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

        public async Task LikeProduct(Like like)
        {
            if(_context.Likes.Contains(like)) 
            {
                _context.Likes.Remove(like);
            }
            else
            {
                await _context.Likes.AddAsync(like);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(i => i.ProductId == id);
        }
        public async Task AddAsync(int sizeId, int quantity, Product product, string imgUrl)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Lưu ProductImage
            ProductImage productImage = new ProductImage 
            {
                ImageUrl = imgUrl,
                ProductId = product.ProductId
            };
            await _context.ProductImages.AddAsync(productImage);

            // Lưu ProductSize
            ProductSize productSize = new ProductSize
            {
                ProductId = product.ProductId,
                SizeId = sizeId,
                Quantity = quantity
            };
            await _context.ProductSizes.AddAsync(productSize);

            //Lưu Database 
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int productId, ProductSize productSize, Product product, string imageUrl)
        {
            
            var productExisting = await _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(i => i.ProductId == productId);
            productExisting.Name = product.Name;
            productExisting.Gender = product.Gender;
            productExisting.UnitPrice = product.UnitPrice;
            productExisting.Color = product.Color;
            productExisting.CategoryId = product.CategoryId;
            productExisting.ProductImages[0].ImageUrl = imageUrl;

            // Cập nhập giá trị ProductSize
            var pdSize = productExisting.ProductSizes
                .FirstOrDefault(i => i.ProductId == productSize.ProductId && i.SizeId == productSize.SizeId);

            if (pdSize != null)
            {
                pdSize.Quantity = productSize.Quantity;
            }
            else
            {
                await _context.ProductSizes.AddAsync(productSize);
            }
                
            // Lưu thay đổi
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

        public bool IsLikeProduct(int productId, string userId)
        {
            Like like = new Like()
            {
                ProductId = productId,
                UserId = userId
            };
            return _context.Likes.Contains(like);
        }

        
    }
}
