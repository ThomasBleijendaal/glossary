using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CosmosDb.Core.Commands;
using CosmosDb.Repositories.Abstractions.Operations;
using CosmosDb.Repositories.Entities;
using CosmosDb.Repositories.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CosmosDb.Repositories.Operations
{
    public class CommentBlogPostOperation : IUpdateOperation<BlogPost>
    {
        private readonly CommentBlogPostCommand _command;

        public CommentBlogPostOperation(CommentBlogPostCommand command)
        {
            _command = command;
        }

        public Expression<Func<BlogPost, bool>> Criteria => post => post.Id == ObjectId.Parse(_command.BlogPostId) && post.IsPublished;

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => null;

        public Func<UpdateDefinitionBuilder<BlogPost>, UpdateDefinition<BlogPost>> Mutation => update => update
            .AddToSet(post => post.Comments, new Comment
            {
                Content = _command.Content
            })
            .Inc(post => post.CommentCount, 1);
    }
}
