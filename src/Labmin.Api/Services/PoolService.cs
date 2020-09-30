using Labmin.Api.Repositories;
using Labmin.Api.Repositories.EfCore;
using Labmin.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labmin.Api.Services
{
    public class PoolService : IPoolService
    {
        private EfCoreRepository<Pool, DbContext> _poolRepository;

        public PoolService(EfCoreRepository<Pool, DbContext> poolRepository)
        {
            _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        }

        public Task<Pool> CreateAsync(Pool pool)
        {
            throw new NotSupportedException();
        }

        public Task<Pool> DeleteAsync(string poolName)
        {
            throw new NotSupportedException();
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
