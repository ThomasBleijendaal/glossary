namespace CQRS.Models
{
    public class BlogPostModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Likes { get; set; }
        public int Comments { get; set; }
    }
}
