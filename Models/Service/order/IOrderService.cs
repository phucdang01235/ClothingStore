using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.order
{
    public interface IOrderService
    {
        Task AddAsync(Order order);
        Task DeleteAsync(int orderId);
        Task UpdateAsync(int id, Order order);
    }
}
