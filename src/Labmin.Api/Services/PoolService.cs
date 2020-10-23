using Labmin.Api.Repositories;
using Labmin.Api.Repositories.EfCore;
using Labmin.Core.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labmin.Api.Services
{
    public class PoolService : IPoolService
    {
        private IRepository<Pool> _poolRepository;

        public PoolService(IRepository<Pool> poolRepository)
        {
            _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        }

        public async Task<Pool> CreateAsync(Pool pool)
        {
            // Ensure entity doesn't exist
            if (!await IsPoolExistsAsync(pool.Name))
            {
                // Doesn't exist, create the entity
                return await _poolRepository.CreateAsync(pool);
            }
            else
            {
                throw new PoolAlreadyExistsException(pool);
            }
        }

        public async Task<Pool> DeleteAsync(string poolName)
        {
            if (await IsPoolExistsAsync(poolName))
            {
                return await _poolRepository.DeleteAsync(poolName);
            }
            else
            {
                throw new PoolNotFoundException(poolName);
            }
        }

        public async Task<bool> IsPoolExistsAsync(string poolName)
        {
            var pool = await _poolRepository.ReadOneAsync(poolName);
            return pool != null;
        }

        public async Task<IEnumerable<Pool>> ReadAllAsync()
        {
            return await _poolRepository.ReadAllAsync();
        }

        public async Task<Pool> ReadOneAsync(string poolName)
        {
            var foundPool = await _poolRepository.ReadOneAsync(poolName);
            if (foundPool != null)
            {
                return foundPool;
            }
            else
            {
                throw new PoolNotFoundException(poolName);
            }
        }

        public async Task<Pool> UpdateAsync(Pool poolWithUpdates)
        {
            if (await IsPoolExistsAsync(poolWithUpdates.Name))
            {
                var poolToUpdate = await ReadOneAsync(poolWithUpdates.Name);
                poolWithUpdates.Id = poolToUpdate.Id;
                var updatedPool = await _poolRepository.UpdateAsync(poolWithUpdates);
                return updatedPool;
            }
            else
            {
                throw new PoolNotFoundException(poolWithUpdates.Name);
            }
        }
    }
}
