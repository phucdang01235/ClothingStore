using ClothingStore.Models.Entity;
using ClothingStore.Models.Service.order;
using ClothingStore.Models.Service.orderdetail;
using ClothingStore.Models.Service.product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;

        private readonly UserManager<User> _userManager;
        public StatisticsController(IProductService productService, UserManager<User> userManager, IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _productService = productService;
            _userManager = userManager;
            _orderDetailService = orderDetailService;
        }

        public async Task<IActionResult> Index()
        {
            // Thống kê sản phẩm bán được trong tháng
            var orderDetails = await _orderDetailService.GetAllAsync();
            var filtersOrderDetail  = orderDetails.Where(o => o.Order.OrderDate.Value.Month == DateTime.Now.Month && o.Order.OrderDate.Value.Year == DateTime.Now.Year).ToList();
            TempData["ProductStatistics"] = filtersOrderDetail.Count;

            // Thống kê đơn hàng bán được trong tháng 
            var orders = await _orderService.GetAllAsync();
            var filtersOrder = orders.Where(o => o.OrderDate.Value.Month == DateTime.Now.Month && o.OrderDate.Value.Year == DateTime.Now.Year).ToList();
            TempData["OrderStatistics"] = filtersOrder.Count;

            // Lấy user có nhiều order nhất
            var userOrderCounts = orders
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalOrders = g.Count()
                });
            var userId = userOrderCounts.OrderByDescending(x => x.TotalOrders).FirstOrDefault()?.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            TempData["TopUser"] = user;

            // Lấy Product có doanh thu cao nhất
            var orderDetailsGroupBy = orderDetails
                .GroupBy(o => o.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Revenue = g.Sum( i => i.Amount)
                });
            var productId = orderDetailsGroupBy.OrderByDescending( i => i.Revenue).FirstOrDefault()?.ProductId;
            var product = await _productService.GetByIdAsync((int)productId);
            TempData["TopProductRevenue"] = product;
            TempData["TotalRevenue"] = orderDetailsGroupBy.OrderByDescending(i => i.Revenue).FirstOrDefault()?.Revenue;

            // Top sản phẩm bán chạy nhất
            var productsGroupBy = orderDetails
                .GroupBy(o => o.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum( i => i.NumberOfProduct)
                });

            var productTop = productsGroupBy.OrderByDescending( i => i.TotalSold);
            TempData["TopProduct"] = productTop;

            

            return View();
        }
    }
}
