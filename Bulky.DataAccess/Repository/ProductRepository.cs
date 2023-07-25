using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class ProductRepository : Repository<Product>, IProductRepository
{
	private readonly ApplicationDbContext _dbContext;

	public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public void Update(Product product)
	{
		_dbContext.Products.Update(product);
	}
}
