using System;
using System.Collections.Generic;

namespace ClothingStore.Models.Entity;

public class Size
{
    public int SizeId { get; set; }

    public string? Name { get; set; }
    public List<ProductSize> ProductSizes { get; set;}
}
