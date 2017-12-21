using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansNetCoreTest.Interfaces;
using System;
using System.Linq;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bankAccounts = await Task.WhenAll(
                (await _factory.GetGrain<IAccount>(0).GetBankAccounts())
                    .Select(async b => await b.Get())
            );

            return Ok(bankAccounts);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var id = await _factory.GetGrain<IAccount>(0).CreateBankAccount();
            return Ok(id);
        }

        [HttpPost("{bankAccountId}/deposit")]
        public async Task<IActionResult> Deposit(Guid bankAccountId, double value)
        {
            var bankAccount = await _factory.GetGrain<IAccount>(0).Get(bankAccountId);
            await bankAccount.Deposit(value);
            return NoContent();
        }

        [HttpPost("{bankAccountId}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid bankAccountId, double value)
        {
            var bankAccount = await _factory.GetGrain<IAccount>(0).Get(bankAccountId);
            await bankAccount.Withdraw(value);
            return NoContent();
        }

        [HttpGet("{bankAccountId}/balance")]
        public async Task<IActionResult> Get(Guid bankAccountId)
        {
            var balance = await _factory.GetGrain<IBankAccount>(bankAccountId).GetBalance();
            return Ok(balance);
        }
    }
}
