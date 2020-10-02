using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Entities;
using CQRS.Handlers;
using CQRS.Models;
using CQRS.Repositories;
using CQRS.Specifications;

namespace CQRS.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IReadRepository<BlogPost> _readRepository;
        private readonly ICommandHandler<CreateBlogPostCommand> _addBlogPostHandler;
        private readonly ICommandHandler<DeleteBlogPostCommand> _deleteBlogPostHandler;

        public BlogPostService(
            IReadRepository<BlogPost> readRepository,
            ICommandHandler<CreateBlogPostCommand> addBlogPostHandler,
            ICommandHandler<DeleteBlogPostCommand> deleteBlogPostHandler)
        {
            _readRepository = readRepository;
            _addBlogPostHandler = addBlogPostHandler;
            _deleteBlogPostHandler = deleteBlogPostHandler;
        }

        public async Task<int> AddBlogPostAsync(string title, string content)
        {
            var command = new CreateBlogPostCommand { Content = content, Title = title };

            await _addBlogPostHandler.HandleAsync(command);

            return command.CreatedId;
        }

        public async Task DeleteBlogPostAsync(int id)
        {
            await _deleteBlogPostHandler.HandleAsync(new DeleteBlogPostCommand { BlogPostId = id });
        }

        public async Task<IEnumerable<BlogPostModel>> GetBlogPostsAsync()
        {
            return await _readRepository.GetListAsync(new BlogPostSpecification());
        }
    }
}
