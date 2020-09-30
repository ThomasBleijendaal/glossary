using System.Collections.Generic;

namespace Repository
{
    public class CompanyHierarchyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<CompanyHierarchyModel> SubCompanies { get; set; }
    }
}
