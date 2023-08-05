namespace Bulky.DataAccess.Repository.Contracts;
public interface IUnitOfWork
{
	ICategoryRepository Categories { get; }
	IProductRepository Products { get; }
	IProductImageRepository ProductImages { get; }
    ICompanyRepository Companies { get; }
	IShoppingCartRepository ShoppingCarts { get; }
	IOrderDetailRepository OrderDetails { get; }
	IOrderHeaderRepository OrderHeaders { get; }
	IApplicationUserRepository ApplicationUsers { get; }
	void Save();
}
