using ClothingStore.Models.DTO;
using ClothingStore.Models;
using ClothingStore.Models.Entity;
using ClothingStore.Models.Helper;
using ClothingStore.Models.Service.order;
using ClothingStore.Models.Service.orderdetail;
using ClothingStore.Models.Service.vnpay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers
{
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IVnPayService _vnPayService;

        public OrderController (
            UserManager<User> userManager,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IVnPayService vnPayService)
        {
            _userManager = userManager;
            _vnPayService = vnPayService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;   
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetOrdersByUserId(user.Id);
            return View(orders);
        }

        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var orderDetails = await _orderDetailService.GetOrderDetailsAsync(user.Id, orderId);


            return View(orderDetails);
        }


        public async Task<IActionResult> PaymentBackReturnUrl()
        {
            
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = "Lỗi thanh toán VNPAY : " + response.VnPayResponseCode;
                return View("OrderFail");
            }

            await PaymentSaveAsync(); // Lưu thông tin Order


            return RedirectToAction("OrderCompleted", "Order");
        }

        public async Task<IActionResult> OrderCompleted()
        {
            return View();
        }

        public async Task<IActionResult> Checkout(string note, string shippingAddress, string payment = "COD")
        {
            List<OrderDetailDTO> orderDetailDTO = HttpContext.Session.GetObjectFromJson<List<OrderDetailDTO>>("OrderDetailDTO");
            var shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart");
            HttpContext.Session.SetObjectAsJson("Note", note);
            HttpContext.Session.SetObjectAsJson("ShippingAddress", shippingAddress);
            var user = await _userManager.GetUserAsync(User);

            if (payment == "Thanh toán VNPAY")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (double)shoppingCart.TotalPrice,
                    CreatedDate = DateTime.Now,
                    Description = note,
                    FullName = user.FullName,
                    OrderId = new Random().Next(1000, 10000)
                };

                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }

            if(payment == "Đặt Hàng (COD)")
            {
                await PaymentSaveAsync(); // Lưu thông tin Order
            }

            return View("OrderCompleted"); // Chưa làm thanh toán momo và thanh toán xong mới lưu
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutCompleting(List<OrderDetailDTO> orderDetailDTO)
        {
            HttpContext.Session.SetObjectAsJson("OrderDetailDTO", orderDetailDTO);

            return View();
        }

        private async Task PaymentSaveAsync()
        {
            List<OrderDetailDTO> orderDetailDTO = HttpContext.Session.GetObjectFromJson<List<OrderDetailDTO>>("OrderDetailDTO");
            var shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart");
            var note = HttpContext.Session.GetObjectFromJson<string>("Note");
            var shippingAddress = HttpContext.Session.GetObjectFromJson<string>("ShippingAddress");
            var user = await _userManager.GetUserAsync(User);

            var order = new Order
            {
                Fullname = user.FullName,
                Email = user.Email,
                OrderDate = DateTime.Now,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
                TotalMoney = (double)shoppingCart.TotalPrice,
                Note = note,
                Address = shippingAddress, // Địa chỉ giao hàng
            };

            // Thêm Order
            await _orderService.AddAsync(order);

            // Thêm OrderDetails
            for (int i = 0; i < orderDetailDTO.Count; i++)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = orderDetailDTO[i].ProductId,
                    NumberOfProduct = orderDetailDTO[i].NumberOfProduct,
                    UnitPrice = orderDetailDTO[i].UnitPrice,
                };
                await _orderDetailService.AddAsync(orderDetail);
            }


        }
    }
}
