using System.ComponentModel.DataAnnotations;

namespace ShopLite.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
