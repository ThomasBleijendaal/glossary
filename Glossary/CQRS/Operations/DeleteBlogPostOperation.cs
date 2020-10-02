using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CQRS.Commands;
using CQRS.Entities;

namespace CQRS.Operations
{
    public class DeleteBlogPostOperation : IOperation<BlogPost>
    {
        private readonly DeleteBlogPostCommand _command;

        public DeleteBlogPostOperation(DeleteBlogPostCommand command)
        {
            _command = command;
        }

        public Expression<Func<BlogPost, bool>> Criteria => post => post.PartitionKey == Program.PartitionKey && post.RowKey == _command.BlogPostId.ToString();

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => null;

        public Action<BlogPost> Mutation => post => post.Deleted = true;

        public Func<BlogPost, bool> Validation => post => post.Deleted;
    }
}
