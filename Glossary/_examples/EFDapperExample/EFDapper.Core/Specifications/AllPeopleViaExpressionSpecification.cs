using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EFDapper.Core.Models;
using EFDapper.Repositories.Abstractions.Specifications;
using EFDapper.Repositories.Entities;
using EFDapper.Repositories.Enums;
using EFDapper.Repositories.Models;

namespace EFDapper.Core.Specifications
{
    public class AllPeopleViaExpressionSpecification : IQueryExpressionSpecification<Person, PersonModel>
    {
        public Expression<Func<Person, bool>> Criteria => person => true;

        public IEnumerable<string>? Includes => new[] { nameof(Person.Addresses), nameof(Person.EmailAddresses) };

        public Expression<Func<Person, PersonModel>> Projection => person => new PersonModel
        {
            Addresses = person.Addresses.Select(address => new AddressModel
            {
                City = address.City,
                Country = address.Country,
                Street = address.Street,
                Zipcode = address.Zipcode
            }),
            EmailAddresses = person.EmailAddresses.Where(email => email.Active).Select(email => new EmailAddressModel
            {
                EmailAddress = email.Emailaddress
            }),
            Name = person.Name
        };

        public IEnumerable<Sort<Person>>? SortingInstructions => new[]
        {
            new Sort<Person>
            {
                KeySelector = person => person.Name,
                SortingDirection = SortingDirection.Ascending }
        };

        public bool SplitQuery => true;
    }
}
