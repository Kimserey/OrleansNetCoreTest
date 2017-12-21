using Orleans;
using Orleans.Streams;
using OrleansNetCoreTest.Interfaces;
using System;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.Grains
{
    [ImplicitStreamSubscription("transactions")]
    public class TransactionLog : Grain, ITransactionLog
    {
        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("transactions");
            var stream = streamProvider.GetStream<TransactionEvent>(this.GetPrimaryKey(), "transactions");
            await stream.SubscribeAsync<TransactionEvent>(OnNext);
        }

        private Task OnNext(TransactionEvent data, StreamSequenceToken token)
        {
            Console.WriteLine(" = = = >> Received {0}", data.Amount);
            return Task.CompletedTask;
        }
    }
}
