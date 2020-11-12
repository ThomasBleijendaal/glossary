using System;

namespace CQRS.Entities
{
    public class Comment
    {
        public DateTime CommentDate { get; set; }
        public string Author { get; set; } = default!;
        public string Text { get; set; } = default!;
    }
}
