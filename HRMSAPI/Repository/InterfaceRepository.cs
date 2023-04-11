namespace HRMSAPI.Repository
{
    public interface InterfaceRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetbyIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
    }
}
