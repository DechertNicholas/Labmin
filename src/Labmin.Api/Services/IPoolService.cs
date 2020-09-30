using Labmin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labmin.Api.Services
{
    public interface IPoolService
    {
        Task<IEnumerable<Pool>> ReadAllAsync();
        Task<Pool> ReadOneAsync(string poolName);
        Task<bool> IsPoolExistsAsync(string poolName);
        Task<Pool> CreateAsync(Pool pool);
        Task<Pool> UpdateAsync(Pool pool);
        Task<Pool> DeleteAsync(string poolName);
    }
}
