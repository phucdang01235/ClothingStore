using System;
using System.Collections.Generic;

namespace ClothingStore.Models.Entity;

public class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public List<Product> Products { get; set; }
}
