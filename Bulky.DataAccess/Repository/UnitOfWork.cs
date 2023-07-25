using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;

namespace Bulky.DataAccess.Repository;
public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;

	private ICategoryRepository Category { get; }
	private IProductRepository Product { get; }

	public UnitOfWork(ApplicationDbContext dbContext,
		ICategoryRepository categoryRepository,
		IProductRepository product)
	{
		_dbContext = dbContext;
		Category = categoryRepository;
		Product = product;
	}

	public ICategoryRepository CategoryRepository => Category;
	public IProductRepository ProductRepository => Product;

	public void Save()
	{
		_dbContext.SaveChanges();
	}
}
