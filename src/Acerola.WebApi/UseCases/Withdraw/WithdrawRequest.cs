namespace Acerola.WebApi.UseCases.Withdraw
{
    public sealed class WithdrawRequest
    {
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}