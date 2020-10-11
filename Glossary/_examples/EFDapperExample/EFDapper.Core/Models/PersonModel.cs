using System.Collections.Generic;

namespace EFDapper.Core.Models
{
    public class PersonModel
    {
        public string Name { get; set; } = default!;

        public IEnumerable<EmailAddressModel> EmailAddresses { get; set; } = default!;

        public IEnumerable<AddressModel> Addresses { get; set; } = default!;
    }
}
