namespace AutoLedger.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        public string? TranType { get; set; }
    }
}
