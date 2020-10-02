using System;
using CQRS.Commands;
using CQRS.Entities;

namespace CQRS.Creations
{
    public class BlogPostCreation : ICreation<BlogPost>
    {
        private readonly CreateBlogPostCommand _command;

        public BlogPostCreation(CreateBlogPostCommand command)
        {
            _command = command;
        }

        public Action<BlogPost> Mutation => post =>
        {
            post.Content = _command.Content;
            post.PublishingDate = _command.PublishDate;
            post.Title = _command.Title;
        };

        public int CreatedId { set => _command.CreatedId = value; }
    }
}
