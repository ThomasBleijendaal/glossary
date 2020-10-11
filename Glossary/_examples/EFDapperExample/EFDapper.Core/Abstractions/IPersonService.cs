using System.Collections.Generic;
using System.Threading.Tasks;
using EFDapper.Core.Models;

namespace EFDapper.Core.Abstractions
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonModel>> GetAllPeopleAsync();

        Task UpdatePersonNameAsync(int id, string newName);
    }
}
