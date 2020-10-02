using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CQRS.Commands;
using CQRS.Entities;

namespace CQRS.Operations
{
    public class AddCommentOperation : IOperation<BlogPost>
    {
        private readonly AddCommentCommand _command;

        public AddCommentOperation(AddCommentCommand command)
        {
            _command = command;
        }

        public Expression<Func<BlogPost, bool>> Criteria => x => x.RowKey == _command.BlogPostId.ToString() && x.PartitionKey == Program.PartitionKey;

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => null;

        public Action<BlogPost> Mutation => post => post.Comments.Add(new Comment { Author = _command.Author, CommentDate = _command.CommentDate, Text = _command.Comment });

        public Func<BlogPost, bool> Validation => post => true;
    }
}
