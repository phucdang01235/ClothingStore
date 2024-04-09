using System;
using System.Collections.Generic;

namespace ClothingStore.Models.Entity;

public class ProductImage
{
    public int ProductImageId { get; set; }

    public string? ImageUrl { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }
}
