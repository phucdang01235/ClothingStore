using ClothingStore.Models.Entity;
using ClothingStore.Models.Helper;
using ClothingStore.Models.Service.product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ClothingStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Detail(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            return View("Detail", product);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> RedirectToProductsView(string gender, string category)
        {
            var products = await _productService.GetProductByCategoryName(category);
            if(gender != null)
            {
                products = products.Where(i => i.Gender.Equals(gender)).ToList();
            }
            HttpContext.Session.SetObjectAsJson("RedirectProducts", products);

            return View("Index", products);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> SortBy(string sortBy = "")
        {
            var products = HttpContext.Session.GetObjectFromJson<IEnumerable<Product>>("RedirectProducts"); ;
            switch (sortBy)
            {
                case "PriceDesc":
                    products = products.OrderByDescending(i => i.UnitPrice);
                    break;
                case "PriceAsc":
                    products = products.OrderBy(i => i.UnitPrice);
                    break;
                case "NameDesc":
                    products = products.OrderByDescending(i => i.Name);
                    break;
                case "NameAsc":
                    products = products.OrderBy(i => i.Name);
                    break;
            }
            
            return View("Index", products);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Search(string keyword = "")
        {
            if (!keyword.IsNullOrEmpty())
            {
                var products = await _productService.Search(keyword);
                HttpContext.Session.SetObjectAsJson("RedirectProducts", products);
                return View("Index", products);
            }
            return RedirectToAction("Index", "Home");

        }

    }
}
