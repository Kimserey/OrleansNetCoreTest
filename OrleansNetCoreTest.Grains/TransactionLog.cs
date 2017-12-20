using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{
    [ImplicitStreamSubscription("transactions")]
    public class TransactionLog : Grain
    {
        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("transactions");
            var stream = streamProvider.GetStream<TransactionEvent>(this.GetPrimaryKey(), "transactions");
            await stream.SubscribeAsync<TransactionEvent>(async (data, token) =>
            {
                Console.WriteLine(data);
            });
        }
    }
}
