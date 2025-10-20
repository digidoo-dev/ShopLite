using System.ComponentModel.DataAnnotations;

namespace ShopLite.Models;

public enum OrderStatus
{
    InProgress,
    Sent
}

public class Order
{
    public int ID { get; set; }


    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.InProgress;
    public decimal TotalPrice { get; set; }


    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
