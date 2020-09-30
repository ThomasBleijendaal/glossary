using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Repository
{
    /**
     * 
     * 
     * uses specification pattern
     * uses generic repository pattern
     * 
     * 
     * TODO:
     * - no entities exposed when returning data
     * - no entities used for inserting data
     * - ISpecification for extending query
     * - Generic repository?
     */

    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, RepositoryApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddDbContext<ExampleDbContext>(config => config.UseInMemoryDatabase("example"));

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IMapper<Company, BasicCompanyModel>, CompanyMapper>();


            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ExampleDbContext>();

            dbContext.Companies.Add(new Company { Name = "Company 1" });
            dbContext.SaveChanges();
        }

        public class RepositoryApp : BaseApp
        {
            private readonly ICompanyRepository _companyRepository;

            public RepositoryApp(ICompanyRepository companyRepository)
            {
                _companyRepository = companyRepository;
            }

            public override async Task Run()
            {
                var companyDetails = await _companyRepository.GetAsync(new DetailedCompanySpecification(1));
            }
        }
    }

    public class BaseEntity
    {
        public int Id { get; set; }
    }

    public class Company : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Company? ParentCompany { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

    public class BasicCompanyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class DetailedCompanyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public BasicCompanyModel? ParentCompany { get; set; }

        public IEnumerable<EmployeeModel> Employees { get; set; }
    }

    public class CompanyHierarchyModel
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public IEnumerable<CompanyHierarchyModel> SubCompanies { get; set; }
    }

    public class EmployeeModel : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasKey(x => x.Id);
            modelBuilder.Entity<Employee>().HasKey(x => x.Id);
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }

    public interface IRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        Task<TModel> GetAsync(int id);
        Task<IEnumerable<TModel>> GetListAsync();
        Task AddAsync(TModel entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(TModel entity);
        Task UpdateAsync(TModel entity);
    }

    public interface ISpecificationRepository<TEntity, TModel> : IRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        Task<TSpecModel> GetAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification);
        Task<IEnumerable<TSpecModel>> GetListAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification);
    }

    public interface ISpecification<TEntity, TModel>
        where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        IEnumerable<string> Includes { get; }
        Expression<Func<TEntity, TModel>> Projection { get; }
    }

    public class DetailedCompanySpecification : ISpecification<Company, DetailedCompanyModel>
    {
        private readonly int _id;

        public DetailedCompanySpecification(int id)
        {
            _id = id;
        }

        public Expression<Func<Company, bool>> Criteria => x => x.Id == _id;

        public IEnumerable<string> Includes => new[] { nameof(Company.Employees) };

        public Expression<Func<Company, DetailedCompanyModel>> Projection => company => new DetailedCompanyModel
        {
            Id = company.Id,
            Name = company.Name,
            ParentCompany = company.ParentCompany == null ? null : new BasicCompanyModel
            {
                Id = company.ParentCompany.Id,
                Name = company.ParentCompany.Name
            },
            Employees = company.Employees.Select(x => new EmployeeModel
            {
                Id = x.Id,
                Name = x.Name
            })
        };
    }

    public interface ICompanyRepository : ISpecificationRepository<Company, BasicCompanyModel>
    {

    }

    public interface IMapper<TEntity, TModel>
    {
        TEntity Map(TModel model);
        TEntity Map(TEntity orignalEntity, TEntity newEntity);
        TModel Map(TEntity entity);
    }

    public class CompanyMapper : IMapper<Company, BasicCompanyModel>
    {
        public Company Map(BasicCompanyModel model)
        {
            return new Company
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public Company Map(Company orignalEntity, Company newEntity)
        {
            orignalEntity.Name = newEntity.Name;
            return orignalEntity;
        }

        public BasicCompanyModel Map(Company entity)
        {
            return new BasicCompanyModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }

    public class Repository<TEntity, TModel> : IRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        protected readonly ExampleDbContext _dbContext;
        protected readonly IMapper<TEntity, TModel> _mapper;

        public Repository(ExampleDbContext dbContext, IMapper<TEntity, TModel> mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddAsync(TModel entity)
        {
            _dbContext.Set<TEntity>().Add(_mapper.Map(entity));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _dbContext.Remove(await _dbContext.Set<TEntity>().Where(x => x.Id == id).AsNoTracking().SingleOrDefaultAsync() ?? throw new Exception("This should be a not found exception."));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TModel entity)
        {
            _dbContext.Remove(_mapper.Map(entity));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetListAsync()
        {
            var entities = await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
            return entities.Select(_mapper.Map);
        }

        public async Task<TModel> GetAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().Where(x => x.Id == id).AsNoTracking().SingleOrDefaultAsync();
            if (entity == null)
            {
                throw new Exception("This should be a not found exception");
            }

            return _mapper.Map(entity);
        }

        public async Task UpdateAsync(TModel entity)
        {
            var updatedEntity = _mapper.Map(entity);
            var originalEntity = await _dbContext.Set<TEntity>().FindAsync(updatedEntity.Id) ?? throw new Exception("This should be a not found exception");

            var mergedEntity = _mapper.Map(originalEntity, updatedEntity);

            _dbContext.Set<TEntity>().Update(mergedEntity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class CompanyRepository : SpecificationRepository<Company, BasicCompanyModel>, ICompanyRepository
    {
        public CompanyRepository(ExampleDbContext dbContext, IMapper<Company, BasicCompanyModel> mapper) : base(dbContext, mapper)
        {
        }
    }

    public class SpecificationRepository<TEntity, TModel> : Repository<TEntity, TModel>, ISpecificationRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        public SpecificationRepository(ExampleDbContext dbContext, IMapper<TEntity, TModel> mapper) : base(dbContext, mapper)
        {
        }

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, IEnumerable<string> includes)
        {
            return includes.Aggregate(query, (query, include) => query.Include(include));
        }

        public async Task<TSpecModel> GetAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification)
        {
            var query = ApplyIncludes(_dbContext.Set<TEntity>(), specification.Includes);
            return await query.Where(specification.Criteria).Select(specification.Projection).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TSpecModel>> GetListAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification)
        {
            var query = ApplyIncludes(_dbContext.Set<TEntity>(), specification.Includes);
            return await query.Where(specification.Criteria).Select(specification.Projection).ToListAsync();
        }
    }
}
