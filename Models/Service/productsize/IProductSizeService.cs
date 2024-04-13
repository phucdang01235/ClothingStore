using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.productsize
{
    public interface IProductSizeService
    {
        Task<IEnumerable<ProductSize>> GetProductSizesByProductId(int productId);
    }
}
