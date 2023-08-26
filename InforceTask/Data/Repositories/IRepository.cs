namespace InforceTask.Data.Repositories;

public interface IRepository<T>
{
    public Task<T> GetOneAsync(int id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<bool> CreateAsync(T item);
    public Task<bool> RemoveAsync(int id);
}