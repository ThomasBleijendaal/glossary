using EFDapper.Core.Models;
using EFDapper.Repositories.Abstractions.Specifications;

namespace EFDapper.Repositories.Specifications
{
    public class BasicPeopleViaSqlSpecification : ISqlSpecification<BasicPersonModel>
    {
        public string Sql => "select * from People";

        public object? Parameters => null;
    }
}
