using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClothingStore.Models.Entity;

public class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public double? UnitPrice { get; set; }

    public string? Color { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public List<ProductImage> ProductImages { get; set; }
    
    public List<Wishlist> Wishlists { get; set; }

    public List<ProductSize> ProductSizes { get; set; }
}
