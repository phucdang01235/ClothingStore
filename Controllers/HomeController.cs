using ClothingStore.Models;
using ClothingStore.Models.Entity;
using ClothingStore.Models.Service.product;
using ClothingStore.Models.Service.wishlist;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ClothingStore.Models.Helper;
using Microsoft.AspNetCore.Authorization;

namespace ClothingStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWishlistService _wishlistService;
        private readonly UserManager<User> _userManager;

        public HomeController(IProductService productService,IWishlistService wishlistService, UserManager<User> userManager)
        {
            _productService = productService;
            _wishlistService = wishlistService;
            _userManager = userManager;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        { 
         
            if (HttpContext.Session.GetObjectFromJson<IEnumerable<Wishlist>>("Wishlists") == null)
            {
                var user = await _userManager.GetUserAsync(User);
                var wishlists = await _wishlistService.GetAllAsync(user.Id);
                HttpContext.Session.SetObjectAsJson("Wishlists", wishlists);
            }

            var productsHome = await _productService.GetAllAsync();

            return View(productsHome);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}