using System;
using EFDapper.Core.Models;
using EFDapper.Repositories.Abstractions.Specifications;

namespace EFDapper.Core.Specifications
{
    public class BasicPeopleViaProjectedSqlSpecification : IProjectedSqlSpecification<BasicPersonModel>
    {
        public Func<dynamic, BasicPersonModel> Projection => person => new BasicPersonModel
        {
            Name = person.Name
        };

        public string Sql => "select * from People";

        public object? Parameters => null;
    }
}
