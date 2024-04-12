using ClothingStore.Models.Entity;
using ClothingStore.Models.Service.order;
using ClothingStore.Models.Service.orderdetail;
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

        public OrderController (
            UserManager<User> userManager,
            IOrderService orderService,
            IOrderDetailService orderDetailService)
        {
            _userManager = userManager;
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

    }
}
