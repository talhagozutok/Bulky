using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart cart);
}
