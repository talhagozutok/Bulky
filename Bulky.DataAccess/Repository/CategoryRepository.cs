using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	private readonly ApplicationDbContext _dbContext;

	public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public void Update(Category category)
	{
		_dbContext.Categories.Update(category);
	}
}
