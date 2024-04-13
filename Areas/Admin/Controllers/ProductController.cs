using ClothingStore.Models.Entity;
using ClothingStore.Models.Service.category;
using ClothingStore.Models.Service.product;
using ClothingStore.Models.Service.productsize;
using ClothingStore.Models.Service.size;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Intrinsics.Arm;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly ISizeService _sizeService;
        private readonly IProductSizeService _productSizeService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IProductService productService,
            ISizeService sizeService,
            IProductSizeService productSizeService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _productSizeService = productSizeService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Detail(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            var sizes = await _productSizeService.GetProductSizesByProductId(productId);

            var viewBagSize = sizes.Select(x => new
            {
                Name = x.Size.Name,
                Quantity = x.Quantity
            });

            ViewBag.Sizes = new SelectList(viewBagSize, "Quantity", "Name");
            
            return View(product);
        }

        public async Task<IActionResult> Add()
        {
            // Tạo ViewBag.Categories
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            // Tạo ViewBag.Sizes
            var sizes = await _sizeService.GetAllAsync();
            ViewBag.Sizes = new SelectList(sizes, "SizeId", "Name");

            // Tạo ViewBag.Gender
            var genders = new[]
            {
                    new SelectListItem { Value = "Nam", Text = "Nam" },
                    new SelectListItem { Value = "Nữ", Text = "Nữ" },
                    new SelectListItem { Value = "Nam, Nữ", Text = "Nam, Nữ" }
            };
            ViewBag.Gender = new SelectList(genders, "Value", "Text");

            return View();
        }

        public async Task<IActionResult> AddProduct(int sizeId, int quantity, Product product, IFormFile image)
        {
            // Kiểm tra hợp lệ
            if(!ModelState.IsValid || sizeId == 0 || product.CategoryId == 0 || quantity < 1)
                return NotFound();


            string imgUrl = await SaveImage(image, product.CategoryId, product.Gender);

            // Lưu database 
            await _productService.AddAsync(sizeId, quantity, product, imgUrl);

            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Update(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            // Tạo ViewBag.Categories
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            // Tạo ViewBag.Sizes
            var sizes = await _sizeService.GetAllAsync();
            ViewBag.Sizes = new SelectList(sizes, "SizeId", "Name");
            
            // Tạo ViewBag.Gender
            var genders = new[]
            {
                    new SelectListItem { Value = "Nam", Text = "Nam" },
                    new SelectListItem { Value = "Nữ", Text = "Nữ" },
                    new SelectListItem { Value = "Nam, Nữ", Text = "Nam, Nữ" }
            };
            ViewBag.Gender = new SelectList(genders, "Value", "Text");

            return View(product);
        }

        public async Task<IActionResult> UpdateProduct(int productId, ProductSize productSize, Product product, IFormFile image)
        {
            // Không truyền đủ tham số
            if(!ModelState.IsValid || productSize.SizeId == 0 || product.CategoryId == 0)
            {
                return NotFound();
            }

            // Lưu Image vào folder
            string imageUrl = await SaveImage(image, product.CategoryId, product.Gender);
            // Cập nhập database cho Product
            await _productService.UpdateAsync(productId, productSize, product, imageUrl);

            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Delete(int productId)
        {
            await _productService.DeleteAsync(productId);
            return RedirectToAction("Index", "Product");
        }

        private async Task<string> SaveImage(IFormFile image, int categoryId, string gender )
        {
            var category = await _categoryService.GetByIdAsync(categoryId);
            string prefixUrl = "/";
            if (gender == "Nam")
            {
                prefixUrl += "nam/";
            }
            else if (gender == "Nữ")
            {
                prefixUrl += "nu/";
            }
            else
            {
                prefixUrl += "phu_kien/";
            }

            switch (category.Name)
            {
                case "Nón - Băng Đô":
                    prefixUrl += "non_bang_do/";
                    break;
                case "Giày":
                    prefixUrl += "giay/";
                    break;
                case "Túi":
                    prefixUrl += "tui/";
                    break;
                case "Áo Polo":
                    prefixUrl += "ao_polo/";
                    break;
                case "Áo Thun":
                    prefixUrl += "ao_thun/";
                    break;
                case "Áo Sơ Mi":
                    prefixUrl += "ao_so_mi/";
                    break;
                case "Áo Khoác":
                    prefixUrl += "ao_khoac/";
                    break;
                case "Áo Vest - Blazer":
                    prefixUrl += "ao_vest_blazer/";
                    break;
                case "Áo Tank Top":
                    prefixUrl += "ao_tank_top/";
                    break;
                case "Áo Len":
                    prefixUrl += "ao_len/";
                    break;
                case "Quần Jean":
                    prefixUrl += "quan_jean/";
                    break;
                case "Quần Kaki":
                    prefixUrl += "quan_kaki/";
                    break;
                case "Quần Vải":
                    prefixUrl += "quan_vai/";
                    break;
                case "Quần Short":
                    prefixUrl += "quan_short/";
                    break;
                case "Quần Jogger":
                    prefixUrl += "quan_jogger/";
                    break;
                case "Ví":
                    prefixUrl += "vi/";
                    break;
                case "Balo":
                    prefixUrl += "balo/";
                    break;
                case "Thắt Lưng":
                    prefixUrl += "that_lung/";
                    break;
                case "Vớ - Tất":
                    prefixUrl += "vo_tat/";
                    break;
                case "Dép":
                    prefixUrl += "dep/";
                    break;
                case "Đầm":
                    prefixUrl += "dam/";
                    break;
                case "Chân Váy":
                    prefixUrl += "chan_vay/";
                    break;
                
            }

            var savePath = Path.Combine("wwwroot/images" + prefixUrl, image.FileName); //Thay đổi đường dẫn theo cấu hình của bạn

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images" + prefixUrl + image.FileName; // Trả về đường dẫn tương đối
        }

    }
}
