using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class CompanyHierarchySpecification : ISpecification<Company, CompanyHierarchyModel>
    {
        public Expression<Func<Company, bool>> Criteria => x => x.Companies.Any();

        public IEnumerable<string> Includes => new[] { nameof(Company.Companies), $"{nameof(Company.Companies)}.{nameof(Company.Companies)}" };

        public Expression<Func<Company, CompanyHierarchyModel>> Projection => company => new CompanyHierarchyModel
        {
            Id = company.Id,
            Name = company.Name,
            SubCompanies = company.Companies.Select(subCompany => new CompanyHierarchyModel
            {
                Id = subCompany.Id,
                Name = subCompany.Name,
                SubCompanies = subCompany.Companies.Select(sub2Company => new CompanyHierarchyModel
                {
                    Id = sub2Company.Id,
                    Name = sub2Company.Name
                })
            })
        };
    }
}
