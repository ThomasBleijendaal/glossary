using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICompanyService
    {
        Task<BasicCompanyModel> GetCompanyAsync(int id);

        Task<DetailedCompanyModel> GetCompanyDetailsAsync(int id);
        Task<IEnumerable<CompanyHierarchyModel>> GetAllCompanyHierarchies();
    }
}
