using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Models.Entity;

public class User : IdentityUser
{
    
    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public bool IsDisable { get; set; } = false;

    public List<Order> Orders { get; set; }

    public List<Wishlist> Wishlists { get; set; }
}
