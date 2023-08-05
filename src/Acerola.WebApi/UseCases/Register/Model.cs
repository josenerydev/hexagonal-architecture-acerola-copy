﻿using Acerola.WebApi.Model;

namespace Acerola.WebApi.UseCases.Register
{
    internal sealed class Model
    {
        public Guid CustomerId { get; }
        public string Personnummer { get; }
        public string Name { get; }
        public List<AccountDetailsModel> Accounts { get; set; }

        public Model(Guid customerId, string perssonnummer, string name, List<AccountDetailsModel> accounts)
        {
            CustomerId = customerId;
            Personnummer = perssonnummer;
            Name = name;
            Accounts = accounts;
        }
    }
}