using System;

namespace CQRS.Commands
{
    public class AddCommentCommand : ICommand
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();

        public string Author { get; set; } = default!;

        public string Comment { get; set; } = default!;

        public DateTime CommentDate { get; set; } = DateTime.UtcNow;

        public int BlogPostId { get; set; }
    }
}
