using System.Collections.Generic;

namespace EFDapper.Repositories.Entities
{
    public class Company
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
