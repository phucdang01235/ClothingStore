using System;
using System.Collections.Generic;

namespace ClothingStore.Models.Entity;

public class Order
{
    public int OrderId { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public double? TotalMoney { get; set; }

    public DateTime? OrderDate { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public List<OrderDetail> OrderDetails { get; set; }
}
