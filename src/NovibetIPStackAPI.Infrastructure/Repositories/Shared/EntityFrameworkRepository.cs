using Microsoft.EntityFrameworkCore;
using NovibetIPStackAPI.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces;
using NovibetIPStackAPI.Core.Interfaces.IPRelated;

namespace NovibetIPStackAPI.Infrastructure.Persistence.Shared
{
    /// <summary>
    /// An implementation of the Entity Framework Core Repository interface.
    /// </summary>
    /// <typeparam name="T">A generic class that must implement the IUpdateable interface.</typeparam>
    public class EntityFrameworkRepository<T> : IEntityFrameworkRepository<T> where T : class, IUpdateable
    {
        private readonly AppDbContext _dbContext;
        public EntityFrameworkRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T GetById(long id)
        {
            return _dbContext.Set<T>().SingleOrDefault(ent => ent.Id == id);
        }
        public Task<T> GetByIdAsync(long id)
        {
            return _dbContext.Set<T>().SingleOrDefaultAsync(ent => ent.Id == id);
        }
        public List<T> List()
        {
            return _dbContext.Set<T>().ToList();
        }
        public Task<List<T>> ListAsync()
        {
            return _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            entity.DateLastModified = DateTime.UtcNow;
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;

        }
        public async Task UpdateAsync(T entity)
        {
            entity.DateLastModified = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(long id)
        {
            T entity = await GetByIdAsync(id);

            await DeleteAsync(entity);
        }

    }
}
