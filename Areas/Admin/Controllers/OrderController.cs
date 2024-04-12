using ClothingStore.Models.Entity;
using ClothingStore.Models.Helper;
using ClothingStore.Models.Service.order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
            if(orders != null)
            {
                HttpContext.Session.SetObjectAsJson("Orders", orders);
            }
            else
            {
                HttpContext.Session.SetObjectAsJson("Orders", new List<Order>());
            }
            
            return RedirectToAction("Index", "User");
        }

    }
}
