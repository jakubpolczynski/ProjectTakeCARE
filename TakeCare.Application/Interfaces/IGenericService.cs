namespace TakeCare.Application.Interfaces
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> ReadAsync(int id);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(TEntity entity, int id);
    }
}