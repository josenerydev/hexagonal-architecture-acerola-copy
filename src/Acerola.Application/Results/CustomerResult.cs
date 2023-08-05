namespace Acerola.Application.Results
{
    public sealed class CustomerResult
    {
        public Guid CustomerId { get; }
        public string Personnummer { get; }
        public string Name { get; }
        public IReadOnlyList<AccountResult> Accounts { get; }

        public CustomerResult(
            Guid customerId,
            string personnummer,
            string name,
            List<AccountResult> accounts)
        {
            CustomerId = customerId;
            Personnummer = personnummer;
            Name = name;
            Accounts = accounts;
        }
    }
}