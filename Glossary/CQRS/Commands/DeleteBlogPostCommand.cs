using System;

namespace CQRS.Commands
{
    public class DeleteBlogPostCommand : ICommand
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();

        public int BlogPostId { get; set; }
    }
}
