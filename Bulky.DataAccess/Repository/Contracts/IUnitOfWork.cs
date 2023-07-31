namespace Bulky.DataAccess.Repository.Contracts;
public interface IUnitOfWork
{
	ICategoryRepository CategoryRepository { get; }
	IProductRepository ProductRepository { get; }
	ICompanyRepository CompanyRepository { get; }
	IShoppingCartRepository ShoppingCartRepository { get; }
	IApplicationUserRepository ApplicationUserRepository { get; }
	void Save();
}
