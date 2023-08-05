namespace Acerola.Application.Results
{
    public sealed class CustomerResult
    {
        public Guid CusomerId { get; }
        public string Pesonnummer { get; }
        public string Name { get; }
        public IReadOnlyList<AccountResult> Accounts { get; }

        public CustomerResult(
            Guid customerId,
            string personnummer,
            string name,
            List<AccountResult> accounts)
        {
            CusomerId = customerId;
            Pesonnummer = personnummer;
            Name = name;
            Accounts = accounts;
        }
    }
}