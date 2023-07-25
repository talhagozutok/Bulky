using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface IProductRepository : IRepository<Product>
{
	void Update(Product product);
}
