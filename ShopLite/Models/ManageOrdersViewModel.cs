namespace ShopLite.Models;

public class ManageOrdersViewModel
{
    public int MonthOrdersCount { get; set; }
    public decimal MonthOrdersTotal { get; set; }
    public decimal AverageOrderValue { get; set; }

    public List<Order> Orders { get; set; }

    public List<string> Last7DaysLabels { get; set; }
    public List<int> Last7DaysCounts { get; set; }
}
