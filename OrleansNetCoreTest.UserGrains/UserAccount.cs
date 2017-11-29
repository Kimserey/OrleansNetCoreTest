using Orleans;
using OrleansNetCoreTest.UserInterfaces;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.UserGrains
{
    public class UserAccount : Grain, IUserAccount
    {
        string _name;

        public Task Create(string name)
        {
            _name = name;
            return Task.CompletedTask;
        }

        public Task<string> GetName()
        {
            return Task.FromResult(_name);
        }
    }
}
