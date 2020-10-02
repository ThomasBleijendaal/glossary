using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CQRS.Commands;
using CQRS.Entities;

namespace CQRS.Operations
{
    public class AddLikeOperation : IOperation<BlogPost>
    {
        private readonly AddLikeCommand _command;

        public AddLikeOperation(AddLikeCommand command)
        {
            _command = command;
        }

        public Expression<Func<BlogPost, bool>> Criteria => x => x.RowKey == _command.BlogPostId.ToString() && x.PartitionKey == Program.PartitionKey;

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => null;

        public Action<BlogPost> Mutation => post => post.Likes++;

        public Func<BlogPost, bool> Validation => post => true;
    }
}
