using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.user
{
    public interface IUserService
    {

        Task<IEnumerable<User>> GetAllAsync();
    }
}
