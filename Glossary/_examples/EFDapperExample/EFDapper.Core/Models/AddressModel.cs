namespace EFDapper.Core.Models
{
    public class AddressModel
    {
        public string Street { get; set; } = default!;

        public string Zipcode { get; set; } = default!;

        public string City { get; set; } = default!;

        public string Country { get; set; } = default!;
    }
}
