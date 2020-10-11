using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFDapper.Repositories.Abstractions;

namespace EFDapper.Repositories.Entities
{
    public class Person : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public ICollection<EmailAddress> EmailAddresses { get; set; } = new List<EmailAddress>();
    }
}
