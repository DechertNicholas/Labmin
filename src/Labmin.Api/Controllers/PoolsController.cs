using Labmin.Api.Services;
using Labmin.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Labmin.Api.Controllers
{
    [Route("v1/[controller]")]
    public class PoolsController : Controller
    {
        private readonly IPoolService _poolService;

        public PoolsController(IPoolService poolService)
        {
            _poolService = poolService ?? throw new ArgumentNullException(nameof(poolService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Pool>), 200)]
        public async Task<IActionResult> ReadAllAsync()
        {
            var allPools = await _poolService.ReadAllAsync();
            return Ok(allPools);
        }
    }
}