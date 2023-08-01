namespace Bulky.DataAccess.Repository.Contracts;
public interface IUnitOfWork
{
	ICategoryRepository CategoryRepository { get; }
	IProductRepository ProductRepository { get; }
	ICompanyRepository CompanyRepository { get; }
	IShoppingCartRepository ShoppingCartRepository { get; }
	IOrderDetailRepository OrderDetailRepository { get; }
	IOrderHeaderRepository OrderHeaderRepository { get; }
	IApplicationUserRepository ApplicationUserRepository { get; }
	void Save();
}
