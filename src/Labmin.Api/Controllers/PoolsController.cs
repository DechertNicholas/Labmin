using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labmin.Api.Data;
using Labmin.Core.Models;
using Labmin.Api.Services;

namespace Labmin.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PoolsController : ControllerBase
    {
        private readonly IPoolService _poolService;

        public PoolsController(IPoolService poolService)
        {
            _poolService = poolService;
        }

        // GET: api/v1/Pools
        [HttpGet]
        public async Task<IEnumerable<Pool>> GetPools()
        {
            return await _poolService.ReadAllAsync();
        }

        // GET: api/v1/Pools/5
        [HttpGet("{name}")]
        public async Task<Pool> GetPool(string name)
        {
            var pool = await _poolService.ReadOneAsync(name);

            return pool;
        }

        //// PUT: api/v1/Pools/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPool(int id, Pool pool)
        //{
        //    if (id != pool.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _poolService.Entry(pool).State = EntityState.Modified;

        //    try
        //    {
        //        await _poolService.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PoolExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/v1/Pools
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Pool>> PostPool(Pool pool)
        //{
        //    _poolService.Pools.Add(pool);
        //    await _poolService.SaveChangesAsync();

        //    return CreatedAtAction("GetPool", new { id = pool.Id }, pool);
        //}

        //// DELETE: api/v1/Pools/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePool(int id)
        //{
        //    var pool = await _poolService.Pools.FindAsync(id);
        //    if (pool == null)
        //    {
        //        return NotFound();
        //    }

        //    _poolService.Pools.Remove(pool);
        //    await _poolService.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool PoolExists(int id)
        //{
        //    return _poolService.Pools.Any(e => e.Id == id);
        //}
    }
}
