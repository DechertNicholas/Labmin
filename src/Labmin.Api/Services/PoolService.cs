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
            if (!IsPoolExistsAsync(pool.Name).Result)
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
            if (IsPoolExistsAsync(poolName).Result)
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

        public Task<IEnumerable<Pool>> ReadAllAsync()
        {
            return _poolRepository.ReadAllAsync();
        }

        public Task<Pool> ReadOneAsync(string poolName)
        {
            return _poolRepository.ReadOneAsync(poolName);
        }

        public Task<Pool> UpdateAsync(Pool pool)
        {
            throw new NotSupportedException();
        }
    }
}
