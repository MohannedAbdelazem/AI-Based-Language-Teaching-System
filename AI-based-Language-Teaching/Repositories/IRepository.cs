using System.Linq.Expressions;

namespace AI_based_Language_Teaching.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ToList();
        Task<T> FindAsync(int id);
        Task Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChanges();

        // New method for querying with a condition
        Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
