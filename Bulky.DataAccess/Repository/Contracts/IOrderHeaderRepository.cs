using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
	void Update(OrderHeader orderHeader);
}
