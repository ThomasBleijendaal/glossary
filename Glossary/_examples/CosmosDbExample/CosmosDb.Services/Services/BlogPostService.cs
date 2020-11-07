using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDb.Core.Commands;
using CosmosDb.Core.Models;
using CosmosDb.Repositories.Abstractions.Repositories;
using CosmosDb.Repositories.Entities;
using CosmosDb.Repositories.Operations;
using CosmosDb.Repositories.Specifications;
using CosmosDb.Services.Abstractions;

namespace CosmosDb.Services.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IReadRepository<BlogPost> _readRepository;
        private readonly IWriteRepository<BlogPost> _writeRepository;

        public BlogPostService(
            IReadRepository<BlogPost> readRepository,
            IWriteRepository<BlogPost> writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<IReadOnlyList<BlogPostModel>> GetTrendingBlogPostsAsync()
        {
            return await _readRepository.GetListAsync(new TrendingBlogPostsSpecification());
        }

        public async Task CommentBlogPostAsync(string blogPostId, string content)
        {
            await _writeRepository.UpdateOneAsync(new CommentBlogPostOperation(new CommentBlogPostCommand
            {
                BlogPostId = blogPostId,
                Content = content
            }));
        }

        public async Task<string> CreateBlogPostAsync(string title, string content)
        {
            var command = new CreateBlogPostCommand
            {
                Content = content,
                Title = title
            };

            await _writeRepository.CreateAsync(new CreateBlogPostOperation(command));

            return command.CreatedBlogPostId ?? throw new Exception();
        }

        public async Task LikeBlogPostAsync(string blogPostId)
        {
            await _writeRepository.UpdateOneAsync(new LikeBlogPostOperation(new LikeBlogPostCommand
            {
                BlogPostId = blogPostId
            }));
        }
    }
}
