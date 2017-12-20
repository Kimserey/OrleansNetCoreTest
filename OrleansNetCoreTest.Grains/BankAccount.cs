using Orleans;
using Orleans.Providers;
using OrleansNetCoreTest.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{

    [StorageProvider(ProviderName = "default")]
    public class BankAccount : Grain<BankAccountState>, IBankAccount
    {
        public async Task Deposit(double a)
        {
            this.State.Balance += a;
            await this.WriteStateAsync();

            try
            {
                var provider = base.GetStreamProvider("transactions");
                var stream = provider.GetStream<TransactionEvent>(this.GetPrimaryKey(), "transactions");
                await stream.OnNextAsync(new TransactionEvent
                {
                    Amount = a,
                    Type = TransactionType.Credit
                });
            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task Withdraw(double a)
        {
            if (a > this.State.Balance)
                throw new ValidationException("Balance cannot be inferior to zero.");

            this.State.Balance -= a;
            await this.WriteStateAsync();


            var provider = base.GetStreamProvider("transactions");
            var stream = provider.GetStream<TransactionEvent>(this.GetPrimaryKey(), "transactions");
            await stream.OnNextAsync(new TransactionEvent
            {
                Amount = a,
                Type = TransactionType.Debit
            });
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
