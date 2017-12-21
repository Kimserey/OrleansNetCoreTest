using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Interfaces
{
    public interface IBankAccount : IGrainWithGuidKey
    {
        Task Deposit(double a);
        Task Withdraw(double a);
        Task<double> GetBalance();
        Task<(Guid, double)> Get();
    }
}
