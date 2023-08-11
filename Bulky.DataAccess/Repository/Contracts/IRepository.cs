using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.Contracts;
public interface IRepository<T> where T : class
{
    // T - Category
    IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null);

    T? Get(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null,
        bool trackChanges = false);

    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
}
