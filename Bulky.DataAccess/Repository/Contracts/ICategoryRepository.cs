using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface ICategoryRepository : IRepository<Category>
{
	void Update(Category category);
}
