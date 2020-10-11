using System.ComponentModel.DataAnnotations;
using EFDapper.Repositories.Abstractions;

namespace EFDapper.Repositories.Entities
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = default!;

        [Required]
        [MaxLength(20)]
        public string Zipcode { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = default!;

        [Required]
        public Person? Person { get; set; }

        public int PersonId { get; set; }
    }
}
