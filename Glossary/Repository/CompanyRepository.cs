namespace Repository
{
    public class CompanyRepository : SpecificationRepository<Company, BasicCompanyModel>, ICompanyRepository
    {
        public CompanyRepository(ExampleDbContext dbContext, IMapper<Company, BasicCompanyModel> mapper) : base(dbContext, mapper)
        {
        }
    }
}
