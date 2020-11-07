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
    public class LikeBlogPostOperation : IUpdateOperation<BlogPost>
    {
        private readonly LikeBlogPostCommand _command;

        public LikeBlogPostOperation(LikeBlogPostCommand command)
        {
            _command = command;
        }

        public Expression<Func<BlogPost, bool>> Criteria => post => post.Id == ObjectId.Parse(_command.BlogPostId) && post.IsPublished;

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => null;

        public Func<UpdateDefinitionBuilder<BlogPost>, UpdateDefinition<BlogPost>> Mutation => update =>
            update.Inc(post => post.Likes, 1);
    }
}
