using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Models.Entity;

public class OrderDetail
{
    
    public int OrderId { get; set; }
    
    public int ProductId { get; set; }

    public double? Amount { get; set; }

    public int? NumberOfProduct { get; set; }

    public double? UnitPrice { get; set; }

    public Order Order { get; set; }

    public Product Product { get; set; }
}
