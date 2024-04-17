using Microsoft.EntityFrameworkCore;
using OnlineRetailShop.Models;
using OnlineRetailShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CommonContext _dbContext;

        public ProductRepository(CommonContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Product.ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _dbContext.Product.FindAsync(id);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            product.Id = Guid.NewGuid();
            _dbContext.Product.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return false;
            }

            _dbContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductAvailable(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _dbContext.Product.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private bool ProductAvailable(Guid id)
        {
            return _dbContext.Product.Any(x => x.Id == id);
        }
    }
}

