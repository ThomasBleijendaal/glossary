using System;

namespace CosmosDb.Core.Commands
{
    public class CommentBlogPostCommand
    {
        public string BlogPostId { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
