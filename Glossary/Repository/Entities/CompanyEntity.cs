using System.Collections.Generic;

namespace Repository
{
    public class CompanyEntity : BaseEntity
    {
        public string? Name { get; set; }

        public CompanyEntity? ParentCompany { get; set; }

        public int? ParentCompanyId { get; set; }

        public ICollection<CompanyEntity> Companies { get; set; } = new List<CompanyEntity>();

        public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
    }
}
