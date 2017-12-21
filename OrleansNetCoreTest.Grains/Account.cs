using Orleans;
using Orleans.Providers;
using OrleansNetCoreTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{
    [StorageProvider(ProviderName = "default")]
    public class Account : Grain<AccountState>, IAccount
    {
        public async Task<Guid> CreateBankAccount()
        {
            var id = Guid.NewGuid();
            var account = GrainFactory.GetGrain<IBankAccount>(id);
            this.State.BankAccounts.Add(account);
            await this.WriteStateAsync();

            return id;
        }

        public Task<List<IBankAccount>> GetBankAccounts()
        {
            return Task.FromResult(this.State.BankAccounts);
        }

        public Task<IBankAccount> Get(Guid id)
        {
            return Task.FromResult(this.State.BankAccounts.Single(b => b.GetPrimaryKey() == id));
        }
    }

    public class AccountState
    {
        public List<IBankAccount> BankAccounts { get; set; }
    }
}
