using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.like
{
    public interface ILikeService
    {
        Task<IEnumerable<Like>> GetAllAsync();
    }
}
