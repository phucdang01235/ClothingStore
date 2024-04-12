using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClothingStore.Models.Entity;

public class ClothingStoreDbContext : IdentityDbContext<User>
{
    public ClothingStoreDbContext(DbContextOptions<ClothingStoreDbContext> options)
        : base(options)
    {
    }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }

    public DbSet<ProductSize> ProductSizes { get; set; }

    public DbSet<Size> Sizes { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        // Xác định khóa chính của OrderDetail
        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderId, od.ProductId });

        // Xác định khóa chính của Wishlist
        modelBuilder.Entity<Wishlist>()
            .HasKey(od => new { od.ProductId, od.UserId });

        // Xác định khóa chính của Wishlist
        modelBuilder.Entity<ProductSize>()
            .HasKey(od => new { od.ProductId, od.SizeId });

        // Xác định khóa chính của Like
        modelBuilder.Entity<Like>()
            .HasKey(od => new { od.ProductId, od.UserId });

        base.OnModelCreating(modelBuilder);
    }


}
