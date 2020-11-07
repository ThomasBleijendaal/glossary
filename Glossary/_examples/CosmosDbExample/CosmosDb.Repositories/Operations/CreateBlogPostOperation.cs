using System;
using CosmosDb.Core.Commands;
using CosmosDb.Repositories.Abstractions.Operations;
using CosmosDb.Repositories.Entities;

namespace CosmosDb.Repositories.Operations
{
    public class CreateBlogPostOperation : ICreationOperation<BlogPost>
    {
        private readonly CreateBlogPostCommand _command;

        public CreateBlogPostOperation(CreateBlogPostCommand command)
        {
            _command = command;
        }

        public Action<BlogPost> Mutation => post =>
        {
            post.Content = _command.Content;
            post.Title = _command.Title;
            post.IsPublished = true;
        };

        public string CreatedId { set => _command.CreatedBlogPostId = value; }
    }
}
