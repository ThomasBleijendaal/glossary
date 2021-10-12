using System.Collections.Generic;

namespace EFDapper.Repositories.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual Company? Company { get; set; }

        public int? CompanyId { get; set; }

        public virtual ICollection<Employee> Minions { get; set; } = new List<Employee>();

        public virtual Employee? Manager { get; set; }

        public int? ManagerId { get; set; }
    }
}
