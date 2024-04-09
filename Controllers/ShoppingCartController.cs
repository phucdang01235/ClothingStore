using ClothingStore.Models.DTO;
using ClothingStore.Models.Entity;
using ClothingStore.Models.Helper;
using ClothingStore.Models.Service.order;
using ClothingStore.Models.Service.orderdetail;
using ClothingStore.Models.Service.product;
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
        public ShoppingCartController(
            UserManager<User> userManager,
            IProductService productService,
            IOrderService ordreService,
            IOrderDetailService orderDetailService)
        {
            _ordreService = ordreService;
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


        public async Task Checkout(string note, string shippingAddress)
        {
            List<OrderDetailDTO> orderDetailDTO = HttpContext.Session.GetObjectFromJson<List<OrderDetailDTO>>("OrderDetailDTO");
            var shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart");
            var user = await _userManager.GetUserAsync(User);
            var order = new Order
            {
                Fullname = user.FullName,
                Email = user.Email,
                OrderDate = DateTime.Now,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
                TotalMoney = (double?)shoppingCart.TotalPrice,
                Note = note,
                Address = shippingAddress, // Địa chỉ giao hàng
            };

            // Thêm Order
            await _ordreService.AddAsync(order);

            // Thêm OrderDetails
            for (int i = 0; i < orderDetailDTO.Count; i++)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = orderDetailDTO[i].ProductId,
                    NumberOfProduct = orderDetailDTO[i].NumberOfProduct,
                    UnitPrice = orderDetailDTO[i].UnitPrice,
                    Amount = orderDetailDTO[i].UnitPrice * orderDetailDTO[i].NumberOfProduct
                };
                await _orderDetailService.AddAsync(orderDetail);
            }

            /*return View();*/ // Chưa làm thanh toán momo và thanh toán xong mới lưu
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutCompleting(List<OrderDetailDTO> orderDetailDTO)
        {
            HttpContext.Session.SetObjectAsJson("OrderDetailDTO", orderDetailDTO);
            return View();
        }

    }
}
