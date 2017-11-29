using Orleans;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Interfaces
{
    public interface IBankAccount : IGrainWithStringKey
    {
        Task Deposit(double a);
        Task Withdraw(double a);
        Task<double> GetBalance();
    }
}
