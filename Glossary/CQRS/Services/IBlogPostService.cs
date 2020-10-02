using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Models;

namespace CQRS.Services
{
    public interface IBlogPostService
    {
        public Task<IEnumerable<BlogPostModel>> GetBlogPostsAsync();
        public Task<int> AddBlogPostAsync(string title, string content);
        public Task DeleteBlogPostAsync(int id);
    }
}
