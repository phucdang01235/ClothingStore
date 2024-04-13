using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.order
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        Task AddAsync(Order order);
        Task DeleteAsync(int orderId);
        Task UpdateAsync(int id, Order order);
    }
}
