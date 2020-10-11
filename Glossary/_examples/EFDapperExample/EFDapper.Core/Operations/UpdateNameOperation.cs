using EFDapper.Repositories.Abstractions.Operations;
using EFDapper.Repositories.Entities;

namespace EFDapper.Core.Operations
{
    public class UpdateNameOperation : ISqlOperation<Person>
    {
        private readonly int _id;
        private readonly string _newName;

        public UpdateNameOperation(int id, string newName)
        {
            _id = id;
            _newName = newName;
        }

        public string Sql => "update People set Name = @Name where Id = @Id";

        public object? Parameters => new { Id = _id, Name = _newName };
    }
}
