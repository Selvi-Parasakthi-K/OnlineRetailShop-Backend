using System.ComponentModel.DataAnnotations;

namespace OnlineRetailShop.Models
{
    public class Customer
    {
        [Key]
        public Guid customerId { get; set; }
        public String customerName { get; set; }
        public String mobile { get; set; }
        public String email { get; set; }
    }
}
