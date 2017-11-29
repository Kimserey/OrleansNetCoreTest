using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansNetCoreTest.UserInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Client.Controllers
{
    [Route("api/userAccounts")]
    public class UserAccountsController: Controller
    {
        private IGrainFactory _factory;

        public UserAccountsController(IGrainFactory factory)
        {
            _factory = factory;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Post(string userId, string name)
        {
            await _factory.GetGrain<IUserAccount>(userId).Create(name);
            return NoContent();
        }

        [HttpGet("{userId}/name")]
        public async Task<IActionResult> GetName(string userId)
        {
            var name = await _factory.GetGrain<IUserAccount>(userId).GetName();
            return Ok(name);
        }
    }
}
