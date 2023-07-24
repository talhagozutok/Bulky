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
	}

	public void Add(T entity)
	{
		dbSet.Add(entity);
	}

	public T? Get(Expression<Func<T, bool>> filter)
	{
		IQueryable<T> query = dbSet;
		query = query.Where(filter);

		return query.FirstOrDefault();
	}

	public IEnumerable<T> GetAll()
	{
		IQueryable<T> query = dbSet;
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
