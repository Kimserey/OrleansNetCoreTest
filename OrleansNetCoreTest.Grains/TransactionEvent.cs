using System.Collections.Generic;

namespace OrleansNetCoreTest.Grains
{
    public class TransactionEvent
    {
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
