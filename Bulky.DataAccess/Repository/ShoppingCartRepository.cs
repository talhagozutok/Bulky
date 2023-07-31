using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(ShoppingCart cart)
    {
        _dbContext.ShoppingCarts.Update(cart);
    }
}
