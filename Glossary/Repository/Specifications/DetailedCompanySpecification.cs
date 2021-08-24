using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class DetailedCompanySpecification : ISpecification<CompanyEntity, DetailedCompanyModel>
    {
        private readonly int _id;

        public DetailedCompanySpecification(int id)
        {
            _id = id;
        }

        public Expression<Func<CompanyEntity, bool>> Criteria => x => x.Id == _id;

        public IEnumerable<string> Includes => new[] { nameof(CompanyEntity.Employees) };

        public Expression<Func<CompanyEntity, DetailedCompanyModel>> Projection => company => new DetailedCompanyModel
        {
            Id = company.Id,
            Name = company.Name,
            ParentCompany = company.ParentCompany == null ? null : new BasicCompanyModel
            {
                Id = company.ParentCompany.Id,
                Name = company.ParentCompany.Name
            },
            Employees = company.Employees.Select(x => new EmployeeEntity
            {
                Id = x.Id,
                Name = x.Name
            })
        };
    }
}
