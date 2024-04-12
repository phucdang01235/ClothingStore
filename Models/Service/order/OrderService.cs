using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.order
{
    public class OrderService : IOrderService
    {
        private readonly ClothingStoreDbContext _context;
        public OrderService(ClothingStoreDbContext context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {
            return await _context.Orders
                .Include(i => i.OrderDetails)
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        
        public Task UpdateAsync(int id, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
