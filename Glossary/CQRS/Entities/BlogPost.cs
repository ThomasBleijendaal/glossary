using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace CQRS.Entities
{
    public class BlogPost : TableEntity
    {
        public BlogPost()
        {
            PartitionKey = Program.PartitionKey;
        }

        [IgnoreProperty]
        public int Id { get => int.Parse(RowKey); set => RowKey = value.ToString(); }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Likes { get; set; }

        [IgnoreProperty]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public string CommentsJson { get => JsonConvert.SerializeObject(Comments); set => Comments = JsonConvert.DeserializeObject<List<Comment>>(value); }

        public DateTime PublishingDate { get; set; }
        public bool Deleted { get; set; }
    }
}
