using Microsoft.EntityFrameworkCore;

namespace OnlineRetailShop.Models
{
    public class CommonContext : DbContext
    {
        public CommonContext(DbContextOptions<CommonContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
    }
}
