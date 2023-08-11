using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class CompanyRepository :
    Repository<Company>,
    ICompanyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Company company)
    {
        _dbContext.Companies.Update(company);
    }
}
