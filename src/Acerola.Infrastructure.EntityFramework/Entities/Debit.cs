namespace Acerola.Infrastructure.Entities
{
    public class Debit
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}