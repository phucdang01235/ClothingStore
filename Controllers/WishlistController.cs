using ClothingStore.Models.Entity;
using ClothingStore.Models.Helper;
using ClothingStore.Models.Service.wishlist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers
{
    [Authorize(Roles = "User")]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly UserManager<User> _userManager;

        public WishlistController(IWishlistService wishlistService, UserManager<User> userManager)
        {
            _wishlistService = wishlistService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetAll()
        {
            return View();
        }

        public async Task<IActionResult> AddToWishlist(int productId) 
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = new Wishlist
            {
                ProductId = productId,
                UserId = user.Id
            };
            await _wishlistService.AddAsync(wishList);

            var wishlists = await _wishlistService.GetAllAsync(user.Id);
            HttpContext.Session.SetObjectAsJson("Wishlists", wishlists);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            // Xóa Wishlist khỏi Session
            var wishlist = HttpContext.Session.GetObjectFromJson<IEnumerable<Wishlist>>("Wishlists");
            var newWishlist = wishlist.Where(i => i.ProductId != productId);
            HttpContext.Session.SetObjectAsJson("Wishlists", newWishlist);

            // Xóa Wishlist khỏi Database
            var user = await _userManager.GetUserAsync(User);
            await _wishlistService.DeleteAsync(user.Id, productId);

            return RedirectToAction("Index", "Home");
        }

    }
}
