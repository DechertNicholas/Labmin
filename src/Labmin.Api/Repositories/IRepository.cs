using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labmin.Api.Data;
using Labmin.Core.Models;

namespace Labmin.Api.Repositories
{
    interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> ReadOneAsync(string name);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(string name);
    }
}
