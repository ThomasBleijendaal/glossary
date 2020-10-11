using System.ComponentModel.DataAnnotations;
using EFDapper.Repositories.Abstractions;

namespace EFDapper.Repositories.Entities
{
    public class EmailAddress : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Emailaddress { get; set; } = default!;

        public bool Active { get; set; } = true;

        public Person? Person { get; set; }

        public int PersonId { get; set; }
    }
}
