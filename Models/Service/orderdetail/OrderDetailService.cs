using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.orderdetail
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ClothingStoreDbContext _context;
        public OrderDetailService(ClothingStoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int orderDetailId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails.Include( i => i.Order).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(string userId, int orderId)
        {
            var orderDetails = await _context.OrderDetails
                .Include(i => i.Product)
                .Include(i => i.Product.ProductImages)
                .Include(i => i.Order)
                .Where(i => i.OrderId == orderId)
                .ToListAsync();
            return orderDetails.Where(i => i.Order.UserId == userId);
        }

        public Task UpdateAsync(int orderDetailId, OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
}
