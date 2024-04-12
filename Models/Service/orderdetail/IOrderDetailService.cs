using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.orderdetail
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(string userId, int orderId);
        Task AddAsync(OrderDetail orderDetail);
        Task DeleteAsync(int orderDetailId);
        Task UpdateAsync(int orderDetailId, OrderDetail orderDetail);
    }
}
