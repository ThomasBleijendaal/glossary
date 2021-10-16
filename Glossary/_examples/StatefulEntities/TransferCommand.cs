namespace StatefulEntities
{
    public class TransferCommand
    {
        public string SourceAccountId { get; set; }
        public string TargetAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
