using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;
public class Repository<T> : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _dbContext;
	internal DbSet<T> dbSet;

	public Repository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		dbSet = _dbContext.Set<T>();
		_dbContext.Products.Include(p => p.Category).Include(p => p.CategoryId);
	}

	public void Add(T entity)
	{
		dbSet.Add(entity);
	}

	public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
	{
		IQueryable<T> query = dbSet;
		query = query.Where(filter);

		if (!string.IsNullOrEmpty(includeProperties))
		{
			foreach (var property in includeProperties
				.Split(',', StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(property);
			}
		}

		return query.FirstOrDefault();
	}

	public IEnumerable<T> GetAll(string? includeProperties = null)
	{
		IQueryable<T> query = dbSet;

		if (!string.IsNullOrEmpty(includeProperties))
		{
			foreach (var property in includeProperties
				.Split(',', StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(property);
			}
		}

		return query.ToList();
	}

	public void Remove(T entity)
	{
		dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entity)
	{
		dbSet.RemoveRange(entity);
	}
}
