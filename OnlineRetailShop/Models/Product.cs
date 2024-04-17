using System.ComponentModel.DataAnnotations;

namespace OnlineRetailShop.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public String productName { get; set; }
        public int quantity { get; set; }
        public bool isActive { get; set; }
    }
}
