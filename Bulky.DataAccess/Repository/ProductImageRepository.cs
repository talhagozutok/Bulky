using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
	private readonly ApplicationDbContext _dbContext;

	public ProductImageRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public void Update(ProductImage image)
	{
		_dbContext.ProductImages.Update(image);
	}
}
