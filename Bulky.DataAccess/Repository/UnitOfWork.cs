using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;

namespace Bulky.DataAccess.Repository;
public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	private ICategoryRepository Category { get; }

	public UnitOfWork(ApplicationDbContext dbContext,
		ICategoryRepository categoryRepository)
	{
		_dbContext = dbContext;
		Category = categoryRepository;
	}

	public ICategoryRepository CategoryRepository => Category;

	public void Save()
	{
		_dbContext.SaveChanges();
	}
}
