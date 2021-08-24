namespace Repository
{
    public class CompanyRepository : SpecificationRepository<CompanyEntity, BasicCompanyModel>, ICompanyRepository
    {
        public CompanyRepository(ExampleDbContext dbContext, IMapper<CompanyEntity, BasicCompanyModel> mapper) : base(dbContext, mapper)
        {
        }
    }
}
