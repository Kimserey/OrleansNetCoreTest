using Orleans;
using Orleans.Providers;
using OrleansNetCoreTest.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{
    [StorageProvider(ProviderName = "default")]
    public class BankAccount : Grain<BankAccountState>, IBankAccount
    {

        public Task Deposit(double a)
        {
            this.State.Balance += a;
            return this.WriteStateAsync();
        }

        public Task Withdraw(double a)
        {
            if (a > this.State.Balance)
                throw new ValidationException("Balance cannot be inferior to zero.");

            this.State.Balance -= a;
            return this.WriteStateAsync();
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
