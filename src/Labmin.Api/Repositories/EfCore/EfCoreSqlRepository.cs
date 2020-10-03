using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labmin.Api.Data;
using Labmin.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Labmin.Api.Repositories.EfCore
{
    public class EfCoreSqlPoolRepository : EfCoreRepository<Pool, DbContext>
    {
        public EfCoreSqlPoolRepository(DbContext context) : base(context)
        {
        }

        //public async Task<Pool> DoesPoolExistAsync(string poolName)
        //{
        //    var pool = await ReadOneAsync(poolName);
        //    return pool ?? null;
        //}

        //public async override Task<Pool> CreateAsync(Pool entity)
        //{
        //    // If the object already exists, perform a no-op
        //    // Names in this application will always be unique, so finding by name is OK

        //    var poolExists = await DoesPoolExistAsync(entity.Name);
        //    if (poolExists != null)
        //    {
        //        return poolExists;
        //    }
        //    else
        //    {
        //        return await base.CreateAsync(entity);
        //    }
        //}

        //public async override Task<Pool> UpdateAsync(Pool entity)
        //{
        //    var poolExists = await DoesPoolExistAsync(entity.Name);
        //    if (poolExists != null)
        //    {
        //        return await base.UpdateAsync(entity);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
