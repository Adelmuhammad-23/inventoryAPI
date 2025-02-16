namespace IMS.Domain.Generic
{
    public interface IGenaricRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddSync(T obj);
        Task<T> UpdatAsync(int id);
        Task<T> DeleteAsync(int id);
    }
}
