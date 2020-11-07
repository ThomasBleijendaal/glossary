namespace CosmosDb.Core.Commands
{
    public class CreateBlogPostCommand
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        public string? CreatedBlogPostId { get; set; }
    }
}
