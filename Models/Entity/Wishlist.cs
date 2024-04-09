using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClothingStore.Models.Entity;

public class Wishlist
{
    public int ProductId { get; set; }

    public string UserId { get; set; }
    public int Quantity { get; set; } = 1;
    public double UnitPrice
    {
        get
        {
            return Product?.UnitPrice ?? 0;
        }
        
    }
    public Product Product { get; set; }

    public User User { get; set; }
}
