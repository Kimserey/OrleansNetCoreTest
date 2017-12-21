using Newtonsoft.Json;
using Orleans;
using Orleans.Streams;
using OrleansNetCoreTest.Interfaces;
using System;
using System.Collections.Generic;
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
            await stream
                .SubscribeAsync<TransactionEvent>(async (@event, token) => {
                    switch (@event.Type)
                    {
                        case TransactionType.Credit: await HandleCredit(@event); break;
                        case TransactionType.Debit: await HandleDebit(@event); break;
                        default: throw new NotSupportedException();
                    }
                });
        }

        public Task HandleCredit(TransactionEvent @event)
        {
            Console.WriteLine(" = = = >> Credited {0}", @event.Amount);
            var x = @event.Data;
            return Task.CompletedTask;
        }

        public Task HandleDebit(TransactionEvent @event)
        {
            Console.WriteLine(" = = = >> Debited {0}", @event.Amount);
            return Task.CompletedTask;
        }
    }
}
