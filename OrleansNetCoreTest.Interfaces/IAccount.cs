using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Interfaces
{
    public interface IAccount: IGrainWithIntegerKey
    {
        Task<Guid> CreateBankAccount();
        Task<List<IBankAccount>> GetBankAccounts();
        Task<IBankAccount> Get(Guid id);
    }
}
