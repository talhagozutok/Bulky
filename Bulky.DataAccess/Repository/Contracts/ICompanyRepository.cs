using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository.Contracts;
public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company company);
}
