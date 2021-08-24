using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class CompanyHierarchySpecification : ISortableSpecification<CompanyEntity, CompanyHierarchyModel>
    {
        public Expression<Func<CompanyEntity, bool>> Criteria => x => x.Companies.Any();

        public IEnumerable<string> Includes => new[] { nameof(CompanyEntity.Companies), $"{nameof(CompanyEntity.Companies)}.{nameof(CompanyEntity.Companies)}" };

        public Expression<Func<CompanyEntity, CompanyHierarchyModel>> Projection => company => new CompanyHierarchyModel
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

        IEnumerable<Sort<CompanyEntity>> ISortableSpecification<CompanyEntity, CompanyHierarchyModel>.SortingInstructions => new[] {
            new Sort<CompanyEntity> { KeySelector = x => x.Id, SortingDirection = SortingDirection.Descending }
        };
    }
}
