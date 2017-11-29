using Orleans;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.UserInterfaces
{
    public interface IUserAccount: IGrainWithStringKey
    {
        Task Create(string name);
        Task<string> GetName();
    }
}
