namespace OrleansNetCoreTest.Grains
{
    public class TransactionEvent
    {
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
    }
}
