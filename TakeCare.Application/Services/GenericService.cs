using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using Couchbase.Core.Exceptions;

namespace TakeCare.Application.Services
{
    public class GenericService<TContext, TEntity> : IGenericService<TEntity> where TContext : DbContext where TEntity : class
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericService(TContext context)
        {
            _context = context ?? throw new InvalidArgumentException("Invalid context");
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentException("Invalid entity provided");
            
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> ReadAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity ?? throw new ArgumentException("Entity not found");
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentException("Invalid entity provided");

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(TEntity entity, int id)
        {
            TEntity? entityExists;
            if(entity != null)
            {
				entityExists = await _dbSet.FindAsync(entity);
			}
            else if(id > 0)
            {
			    entityExists = await _dbSet.FindAsync(id);
            }
            else throw new ArgumentException("Invalid entity or id provided");

			return entityExists != null;
        }
    }
}
