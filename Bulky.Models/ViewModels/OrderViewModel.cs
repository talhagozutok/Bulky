using Bulky.Models.Entities;

namespace Bulky.Models.ViewModels;
public class OrderViewModel
{
    public OrderHeader OrderHeader { get; set; }
    public IEnumerable<OrderDetail> OrderDetails { get; set; }
}
