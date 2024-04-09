using ClothingStore.Models.Entity;

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

        public Task UpdateAsync(int orderDetailId, OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
}
