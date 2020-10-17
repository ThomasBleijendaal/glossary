using System.Collections.Generic;
using System.Threading.Tasks;
using EFDapper.Core.Models;
using EFDapper.Repositories.Abstractions.Repositories;
using EFDapper.Repositories.Entities;
using EFDapper.Repositories.Operations;
using EFDapper.Repositories.Specifications;
using EFDapper.Services.Abstractions;

namespace EFDapper.Services.Services
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
