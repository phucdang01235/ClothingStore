using System;
using System.Collections.Generic;

namespace ClothingStore.Models.Entity;

public class ProductSize
{
    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int Quantity { get; set; }

    public Product Product { get; set; }

    public Size Size { get; set; }
}
