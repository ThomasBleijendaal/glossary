using System.Collections.Generic;
using System.Threading.Tasks;
using EFDapper.Core.Abstractions;
using EFDapper.Core.Models;
using EFDapper.Core.Operations;
using EFDapper.Core.Specifications;
using EFDapper.Repositories.Abstractions.Repositories;
using EFDapper.Repositories.Entities;

namespace EFDapper.Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly IReadRepository<Person> _personRepository;
        private readonly IWriteRepository<Person> _personWriteRepository;

        public PersonService(
            IReadRepository<Person> personRepository,
            IWriteRepository<Person> personWriteRepository)
        {
            _personRepository = personRepository;
            _personWriteRepository = personWriteRepository;
        }

        public async Task<IEnumerable<PersonModel>> GetAllPeopleAsync()
        {
            return await _personRepository.GetListAsync(new AllPeopleViaExpressionSpecification());
        }

        public async Task UpdatePersonNameAsync(int id, string newName)
        {
            await _personWriteRepository.ExecuteAsync(new UpdateNameOperation(id, newName));
        }
    }
}
