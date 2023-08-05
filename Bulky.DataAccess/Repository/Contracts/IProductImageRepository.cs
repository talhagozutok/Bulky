using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface IProductImageRepository : IRepository<ProductImage>
{
	void Update(ProductImage image);
}
