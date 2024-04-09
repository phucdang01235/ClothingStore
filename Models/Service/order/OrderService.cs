using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.order
{
    public class OrderService : IOrderService
    {
        private readonly ClothingStoreDbContext _context;
        public OrderService(ClothingStoreDbContext context) 
        { 
            _context = context;
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
