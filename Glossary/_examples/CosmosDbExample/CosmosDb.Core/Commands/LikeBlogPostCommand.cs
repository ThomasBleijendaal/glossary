using System;

namespace CosmosDb.Core.Commands
{
    public class LikeBlogPostCommand
    {
        public string BlogPostId { get; set; } = default!;
    }
}
