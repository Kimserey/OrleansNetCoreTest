using Newtonsoft.Json;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansNetCoreTest.Grains
{
    public class TransactionEvent
    {
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
