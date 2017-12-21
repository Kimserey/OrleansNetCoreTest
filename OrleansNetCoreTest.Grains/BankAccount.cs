using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using OrleansNetCoreTest.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{
    [StorageProvider(ProviderName = "Default")]
    public class BankAccount : Grain<BankAccountState>, IBankAccount
    {
        private IAsyncStream<TransactionEvent> _stream;

        public override Task OnActivateAsync()
        {
            var provider = base.GetStreamProvider("transactions");
            _stream = provider.GetStream<TransactionEvent>(this.GetPrimaryKey(), "transactions");
            return base.OnActivateAsync();
        }

        public async Task Deposit(double a)
        {
            this.State.Balance += a;
            await this.WriteStateAsync();

            await _stream.OnNextAsync(new TransactionEvent
            {
                Amount = a,
                Type = TransactionType.Credit,
                Data = new Dictionary<string, string> { { "key", "hello" } }
            });
        }

        public async Task Withdraw(double a)
        {
            if (a > this.State.Balance)
                throw new ValidationException("Balance cannot be inferior to zero.");

            this.State.Balance -= a;
            await this.WriteStateAsync();

            await _stream.OnNextAsync(new TransactionEvent
            {
                Amount = a,
                Type = TransactionType.Debit
            });
        }

        public Task<(Guid, double)> Get()
        {
            return Task.FromResult((this.GetPrimaryKey(), this.State.Balance));
        }

        public Task<double> GetBalance()
        {
            return Task.FromResult(this.State.Balance);
        }
    }

    public class BankAccountState
    {
        public double Balance { get; set; }
    }
}
