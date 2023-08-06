using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail orderDetail);
}
