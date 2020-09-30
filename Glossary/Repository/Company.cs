using System.Collections.Generic;

namespace Repository
{
    public class Company : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Company? ParentCompany { get; set; }

        public int? ParentCompanyId { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
