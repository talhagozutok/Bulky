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
		var productFromDb = _dbContext.Products.Find(product.Id);

		if (productFromDb is not null)
		{
			productFromDb.Title = product.Title;
			productFromDb.ISBN = product.ISBN;
			productFromDb.ListPrice = product.ListPrice;
			productFromDb.PriceFifty = product.PriceFifty;
			productFromDb.PriceHundredOrMore = product.PriceHundredOrMore;
			productFromDb.Description = product.Description;
			productFromDb.CategoryId = product.CategoryId;
			productFromDb.Author = product.Author;
			productFromDb.ProductImages = product.ProductImages;
        }
	}
}
