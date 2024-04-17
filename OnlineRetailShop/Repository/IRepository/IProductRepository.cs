using OnlineRetailShop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRetailShop.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(Guid id);
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Guid id, Product product);
        Task<bool> DeleteProduct(Guid id);
    }
}

