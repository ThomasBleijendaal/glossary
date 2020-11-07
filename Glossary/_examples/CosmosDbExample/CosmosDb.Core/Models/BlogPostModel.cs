using System.Collections.Generic;

namespace CosmosDb.Core.Models
{
    public class BlogPostModel
    {
        public string Id { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;

        public int Likes { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; } = default!;
    }
}
