
using ClothingStore.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Models.Entity
{
    public class ShoppingCart
    {
        public List<Wishlist> Items { get; set; } = new List<Wishlist>();

        public void AddItem(Wishlist item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }

        }
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }

        public void ClearAll()
        {
            Items.RemoveAll(i => i != null);
        }

        public decimal TotalPrice => (decimal)Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
