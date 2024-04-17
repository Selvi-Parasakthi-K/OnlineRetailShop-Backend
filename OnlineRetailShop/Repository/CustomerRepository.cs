using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineRetailShop.Models;
using OnlineRetailShop.Repository;

namespace OnlineRetailShop.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CommonContext _dbContext;

        public CustomerRepository(CommonContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _dbContext.Customer.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await _dbContext.Customer.FindAsync(id);
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            customer.customerId = Guid.NewGuid();
            _dbContext.Customer.Add(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateCustomer(Guid id, Customer customer)
        {
            if (id != customer.customerId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _dbContext.Entry(customer).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    throw new KeyNotFoundException($"Customer with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteCustomer(Guid id)
        {
            var customer = await _dbContext.Customer.FindAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }
            _dbContext.Customer.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }

        private bool CustomerExists(Guid id)
        {
            return _dbContext.Customer.Any(e => e.customerId == id);
        }
    }
}

