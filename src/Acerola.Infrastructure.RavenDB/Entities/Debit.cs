namespace Acerola.Infrastructure.Entities
{
    public class Debit
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}