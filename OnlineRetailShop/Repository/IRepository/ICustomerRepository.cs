using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineRetailShop.Models;

namespace OnlineRetailShop.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(Guid id);
        Task<Customer> CreateCustomer(Customer customer);
        Task UpdateCustomer(Guid id, Customer customer);
        Task DeleteCustomer(Guid id);
    }
}

