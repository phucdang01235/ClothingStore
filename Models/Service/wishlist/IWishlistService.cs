using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.wishlist
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetAllAsync(string userId);
        Task AddAsync(Wishlist wishlist);
        Task DeleteAsync(string userId, int productId);
    }
}
