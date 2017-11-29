﻿using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansNetCoreTest.Interfaces;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Client.Controllers
{
    [Route("api/bankAccounts")]
    public class BankAccountsController : Controller
    {
        private IGrainFactory _factory;

        public BankAccountsController(IGrainFactory factory)
        {
            _factory = factory;
        }

        [HttpPost("{bankAccountId}/deposit")]
        public async Task<IActionResult> Deposit(string bankAccountId, double value)
        {
            await _factory.GetGrain<IBankAccount>(bankAccountId).Deposit(value);
            return NoContent();
        }

        [HttpPost("{bankAccountId}/withdraw")]
        public async Task<IActionResult> Withdraw(string bankAccountId, double value)
        {
            await _factory.GetGrain<IBankAccount>(bankAccountId).Withdraw(value);
            return NoContent();
        }

        [HttpGet("{bankAccountId}/balance")]
        public async Task<IActionResult> Get(string bankAccountId)
        {
            var balance = await _factory.GetGrain<IBankAccount>(bankAccountId).GetBalance();
            return Ok(balance);
        }
    }
}
