﻿using ClothingStore.Models.Entity;

namespace ClothingStore.Models.Service.product
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetProductByCategoryName(string category);
        Task<IEnumerable<Product>> Search(string keyword);
        Task AddAsync(Product product);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Product product);
    }
}