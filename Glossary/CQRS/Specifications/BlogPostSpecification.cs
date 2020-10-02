using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CQRS.Entities;
using CQRS.Models;

namespace CQRS.Specifications
{
    public class BlogPostSpecification : ISpecification<BlogPost, BlogPostModel>
    {
        public Expression<Func<BlogPost, bool>> Criteria => x => !x.Deleted;

        public IEnumerable<string> Includes => new[] { nameof(BlogPost.Comments) };

        public Expression<Func<BlogPost, BlogPostModel>> Projection => post => new BlogPostModel
        {
            Comments = post.Comments.Count,
            Content = post.Content,
            Id = post.Id,
            Likes = post.Likes,
            Title = post.Title
        };

        public IEnumerable<Sort<BlogPost>> SortingInstructions
            => new[] { new Sort<BlogPost> { KeySelector = post => post.PublishingDate, SortingDirection = SortingDirection.Ascending } };
    }
}
