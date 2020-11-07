using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CosmosDb.Core.Models;
using CosmosDb.Repositories.Abstractions.Specifications;
using CosmosDb.Repositories.Entities;
using CosmosDb.Repositories.Enums;
using CosmosDb.Repositories.Models;

namespace CosmosDb.Repositories.Specifications
{
    public class TrendingBlogPostsSpecification : ISpecification<BlogPost, BlogPostModel>
    {
        public Expression<Func<BlogPost, bool>> Criteria => post => post.IsPublished;

        public Expression<Func<BlogPost, BlogPostModel>> Projection => post => new BlogPostModel
        {
            Comments = post.Comments.Select(comment => new CommentModel
            {
                Content = comment.Content
            }),
            Content = post.Content,
            Id = post.Id.ToString(),
            Likes = post.Likes,
            Title = post.Title
        };

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => new[] {
            new Sort<BlogPost>
            {
                KeySelector = post => post.Likes,
                SortingDirection = SortingDirection.Descending
            },
            new Sort<BlogPost>
            {
                KeySelector = post => post.CommentCount,
                SortingDirection = SortingDirection.Ascending
            }
        };
    }
}
