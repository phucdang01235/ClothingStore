using ClothingStore.Models;
using ClothingStore.Models.DTO;
using ClothingStore.Models.Entity;
using ClothingStore.Models.Helper;
using ClothingStore.Models.Service.order;
using ClothingStore.Models.Service.orderdetail;
using ClothingStore.Models.Service.product;
using ClothingStore.Models.Service.vnpay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace ClothingStore.Controllers
{
    [Authorize(Roles = "User")]
    public class ShoppingCartController : Controller
    {
        private readonly IOrderService _ordreService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly UserManager<User> _userManager;
        private readonly IProductService _productService;
        private readonly IVnPayService _vnPayService;
        public ShoppingCartController(
            UserManager<User> userManager,
            IProductService productService,
            IOrderService ordreService,
            IOrderDetailService orderDetailService,
            IVnPayService vnPayService)
        {
            _ordreService = ordreService;
            _vnPayService = vnPayService;
            _orderDetailService = orderDetailService;
            _userManager = userManager;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToShoppingCartFromProduct(int productId)
        {
            // Lấy Session từ ShoppingCart
            var shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart") ?? new ShoppingCart();
            // Lấy User hiện tại
            var user = await _userManager.GetUserAsync(User);

            // Lấy sản phẩm hiện tại
            var product = await _productService.GetByIdAsync(productId);

            // Thêm sản phẩm từ Wishlist đến ShoppingCart
            var wishlist = new Wishlist
            {
                UserId = user.Id,
                ProductId = productId,
                Product = product,
                Quantity = 1
            };

            shoppingCart.AddItem(wishlist);

            // Lưu lại Shoppingcart vào Session
            HttpContext.Session.SetObjectAsJson("ShoppingCart", shoppingCart);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart(int productId, int quantity)
        {
            // Lấy Session từ ShoppingCart
            var shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart") ?? new ShoppingCart();

            // Lấy User hiện tại
            var user = await _userManager.GetUserAsync(User);

            // Lấy sản phẩm hiện tại
            var product = await _productService.GetByIdAsync(productId);

            // Thêm sản phẩm từ Wishlist đến ShoppingCart
            var wishlist = new Wishlist
            {
                UserId = user.Id,
                ProductId = productId,
                Product = product,
                Quantity = quantity
            };
            shoppingCart.AddItem(wishlist);

            // Lưu lại Shoppingcart vào Session
            HttpContext.Session.SetObjectAsJson("ShoppingCart", shoppingCart);

            // Xóa Product khỏi Wishlist kiểm tra lại
            var wishlistOld = HttpContext.Session.GetObjectFromJson<IEnumerable<Wishlist>>("Wishlists");
            var wishlistNew = wishlistOld.Where(x => x.ProductId != productId).ToList();
            HttpContext.Session.SetObjectAsJson("Wishlists", wishlistNew);

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> RemoveFromShoppingCart(int productId)
        {
            // Xóa CartItem từ ShoppingCart khỏi Session
            var shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart");
            shoppingCart.RemoveItem(productId);
            HttpContext.Session.SetObjectAsJson("ShoppingCart", shoppingCart);

            return RedirectToAction("Index", "Home");
        }


 

    }
}
