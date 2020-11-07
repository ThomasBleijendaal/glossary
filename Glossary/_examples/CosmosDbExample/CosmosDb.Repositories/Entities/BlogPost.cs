using System.Collections.Generic;
using CosmosDb.Repositories.Abstractions;
using MongoDB.Bson;

namespace CosmosDb.Repositories.Entities
{
    public class BlogPost : IEntity
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        public int Likes { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public int CommentCount { get; set; }
    }
}
