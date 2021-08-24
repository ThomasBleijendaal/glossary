using System.Collections.Generic;

namespace Repository
{
    public class DetailedCompanyModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public BasicCompanyModel? ParentCompany { get; set; }

        public IEnumerable<EmployeeEntity>? Employees { get; set; }
    }
}
