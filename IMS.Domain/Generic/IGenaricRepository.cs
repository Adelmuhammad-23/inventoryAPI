using System.Linq.Expressions;

namespace IMS.Domain.Generic
{
    public interface IGenaricRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(
                    params Expression<Func<T, object>>[] includes);
        public Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<T> AddSync(T obj);
        Task<T> UpdatAsync(int id);
        Task<T> DeleteAsync(int id);
    }
}
