using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labmin.Api.Data;
using Labmin.Core.Models;

namespace Labmin.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PoolsController : ControllerBase
    {
        private readonly LabminDbContext _context;

        public PoolsController(LabminDbContext context)
        {
            _context = context;
        }

        // GET: api/Pools1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pool>>> GetPools()
        {
            return await _context.Pools.ToListAsync();
        }

        // GET: api/Pools1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pool>> GetPool(int id)
        {
            var pool = await _context.Pools.FindAsync(id);

            if (pool == null)
            {
                return NotFound();
            }

            return pool;
        }

        // PUT: api/Pools1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPool(int id, Pool pool)
        {
            if (id != pool.Id)
            {
                return BadRequest();
            }

            _context.Entry(pool).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pools1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pool>> PostPool(Pool pool)
        {
            _context.Pools.Add(pool);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPool", new { id = pool.Id }, pool);
        }

        // DELETE: api/Pools1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePool(int id)
        {
            var pool = await _context.Pools.FindAsync(id);
            if (pool == null)
            {
                return NotFound();
            }

            _context.Pools.Remove(pool);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PoolExists(int id)
        {
            return _context.Pools.Any(e => e.Id == id);
        }
    }
}
