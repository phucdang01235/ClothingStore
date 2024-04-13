using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.size
{
    public interface ISizeService
    {
        Task<IEnumerable<Size>> GetAllAsync();
    }
}
