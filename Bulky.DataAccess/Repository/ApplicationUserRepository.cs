using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class ApplicationUserRepository : Repository<Category>, IApplicationUserRepository
{
	private readonly ApplicationDbContext _dbContext;

	public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
