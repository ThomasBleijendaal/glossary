using System.Threading.Tasks;

namespace CQRS.Services
{
    public interface ICommentService
    {
        public Task AddLikeAsync(int blogPostId);
        public Task AddCommentAsync(int blogPostId, string author, string comment);
    }
}
