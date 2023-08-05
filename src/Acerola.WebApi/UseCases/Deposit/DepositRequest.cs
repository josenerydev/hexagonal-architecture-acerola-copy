namespace Acerola.WebApi.UseCases.Deposit
{
    public sealed class DepositRequest
    {
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}