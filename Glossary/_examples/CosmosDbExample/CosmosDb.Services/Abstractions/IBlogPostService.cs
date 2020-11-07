using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDb.Core.Models;

namespace CosmosDb.Services.Abstractions
{
    public interface IBlogPostService
    {
        Task<IReadOnlyList<BlogPostModel>> GetTrendingBlogPostsAsync();
        Task<string> CreateBlogPostAsync(string title, string content);
        Task LikeBlogPostAsync(string blogPostId);
        Task CommentBlogPostAsync(string blogPostId, string content);
    }
}
