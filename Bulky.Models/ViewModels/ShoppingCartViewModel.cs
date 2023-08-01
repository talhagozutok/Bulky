using Bulky.Models.Entities;

namespace Bulky.Models.ViewModels;
public class ShoppingCartViewModel
{
    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
