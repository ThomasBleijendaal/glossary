using System;

namespace CQRS.Commands
{
    public class CreateBlogPostCommand : ICommand
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public int CreatedId { get; set; }
    }
}
