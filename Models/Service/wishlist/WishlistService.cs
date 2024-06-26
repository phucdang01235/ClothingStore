﻿using ClothingStore.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models.Service.wishlist
{
    public class WishlistService : IWishlistService
    {
        private readonly ClothingStoreDbContext _context;
        public WishlistService(ClothingStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Wishlist wishlist)
        {
            var wishlists = await _context.Wishlists.ToListAsync();
            var wishlistExisting =  wishlists.FirstOrDefault(i => i.ProductId == wishlist.ProductId && i.UserId == wishlist.UserId);
            if (wishlistExisting == null)
            {
                _context.Wishlists.Add(wishlist);
            }
            else
            {
                wishlistExisting.Quantity += 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId ,int productId)
        {
            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(i => i.UserId.Equals(userId) && i.ProductId == productId);
            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Where(i => i.UserId.Equals(userId))
                .Include(i => i.Product)
                .Include(i => i.Product.ProductImages)
                .ToListAsync();
            return wishlist;
        }
    }
}
