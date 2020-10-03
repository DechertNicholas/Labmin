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

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var foundEntity = await ReadOneAsync(entity.Name);
            // Ensure entity doesn't exist
            if (foundEntity == null)
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
                foundEntity = await ReadOneAsync(entity.Name);
                return foundEntity;
            }
            else
            {
                return foundEntity;
            }
            
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            // For Update, we need to search by Id, not any other property
            // because the property could be changing
            var foundEntity = await _context.Set<TEntity>().FindAsync(entity.Id);
            if (foundEntity == null)
            {
                return null;
            }

            _context.Entry(foundEntity).State = EntityState.Modified;
            _context.Entry(foundEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            var updatedEntity = await ReadOneAsync(entity.Name);
            
            return updatedEntity;
        }

        public async Task<TEntity> DeleteAsync(string name)
        {
            var foundEntity = await ReadOneAsync(name);
            if (foundEntity == null)
            {
                // If entity does not exist, return null (error in service layer)
                return null;
            }

            _context.Set<TEntity>().Remove(foundEntity);
            await _context.SaveChangesAsync();

            // Confirm deletion
            var nullEntity = await ReadOneAsync(name);
            if (nullEntity == null)
            {
                // If entity does not exist, return the entity we deleted
                return foundEntity;
            }

            // Otherwise, return null (error in service layer)
            return null;
        }
    }
}
