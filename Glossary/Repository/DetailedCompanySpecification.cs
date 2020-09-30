using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
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
}
