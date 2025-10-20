using System.ComponentModel.DataAnnotations;

namespace ShopLite.Models
{
    public class OrderProduct
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }


        public int ProductID { get; set; }
        public Product Product { get; set; }


        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

    }
}
