using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public Task<IEnumerable<CompanyHierarchyModel>> GetAllCompanyHierarchies()
        {
            return _companyRepository.GetListAsync(new CompanyHierarchySpecification());
        }

        public Task<BasicCompanyModel> GetCompanyAsync(int id)
        {
            return _companyRepository.GetAsync(id);
        }

        public Task<DetailedCompanyModel> GetCompanyDetailsAsync(int id)
        {
            return _companyRepository.GetAsync(new DetailedCompanySpecification(1));
        }
    }
}
