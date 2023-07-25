namespace Bulky.DataAccess.Repository.Contracts;
public interface IUnitOfWork
{
	ICategoryRepository CategoryRepository { get; }
	IProductRepository ProductRepository { get; }
	void Save();
}
