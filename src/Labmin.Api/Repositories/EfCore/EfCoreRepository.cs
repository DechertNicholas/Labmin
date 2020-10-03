using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labmin.Api.Data;
using Labmin.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Labmin.Api.Repositories.EfCore
{
    public abstract class EfCoreRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private readonly TContext _context;

        public EfCoreRepository(TContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> ReadAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> ReadOneAsync(string name)
        {
            var pool = await _context.Set<TEntity>().FirstOrDefaultAsync(n => n.Name == name);
            return await Task.FromResult(pool);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            var updatedEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(n => n.Name == entity.Name);
            
            return updatedEntity;
        }

        public async Task<TEntity> DeleteAsync(string name)
        {
            var entity = await ReadOneAsync(name);
            if (entity == null)
            {
                return entity;
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
