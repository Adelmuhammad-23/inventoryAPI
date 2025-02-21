using System.Linq.Expressions;

namespace IMS.Domain.Generic
{
    public interface IGenaricRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(
                    params Expression<Func<T, object>>[] includes);
        public Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T obj);
        Task<T> UpdatAsync(T obj);
        Task<T> DeleteAsync(int id);
    }
}
