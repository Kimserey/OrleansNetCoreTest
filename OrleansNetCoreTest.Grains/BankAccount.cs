using Orleans;
using OrleansNetCoreTest.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{
    public class BankAccount : Grain, IBankAccount
    {
        double _balance;

        public Task Deposit(double a)
        {
            _balance += a;
            return Task.CompletedTask;
        }

        public Task Withdraw(double a)
        {
            if (a > _balance)
                throw new ValidationException("Balance cannot be inferior to zero.");

            _balance -= a;
            return Task.CompletedTask;
        }

        public Task<double> GetBalance()
        {
            return Task.FromResult(_balance);
        }
    }
}
