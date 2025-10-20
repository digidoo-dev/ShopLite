using System.ComponentModel.DataAnnotations;

namespace ShopLite.Models
{
    public class Product
    {
        public int ID { get; set; }
        

        public int CategoryID { get; set; }
        public Category? Category { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        
        
        [StringLength(255)]
        public string? ImageUrl { get; set; }


        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }


        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
