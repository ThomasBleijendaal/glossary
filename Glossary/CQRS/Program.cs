using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;
using ZCommon;

namespace RepositoryCQRS
{
    /**
     * 
     * must contain:
     * - cached reads
     * - command handler style writing
     * 
     * - queued commands
     * 
     * - give like
     * - post comment
     * - save new post
     */

    public class Program : BaseProgram
    {
        public const string PartitionKey = "1";

        public static async Task Main(string[] args)
        {
            await Init<Program, RepositoryCQRS>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddSingleton(CloudStorageAccount.DevelopmentStorageAccount);
        }

        public class RepositoryCQRS : BaseApp
        {
            private readonly IReadRepository<BlogPost> _blogRepository;

            public RepositoryCQRS(IReadRepository<BlogPost> blogRepository)
            {
                _blogRepository = blogRepository;
            }

            public override async Task Run()
            {
                var posts = await _blogRepository.GetListAsync(new BlogPostSpecification());


            }
        }
    }

    public class BlogPost : TableEntity
    {
        public BlogPost()
        {
            PartitionKey = Program.PartitionKey;
        }

        [IgnoreProperty]
        public int Id { get => int.Parse(RowKey); set => RowKey = value.ToString(); }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public DateTime PublishingDate { get; set; }
        public bool Deleted { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost Post { get; set; }
    }

    public class BlogPostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
    }

    public class BlogPostSpecification : ISpecification<BlogPost, BlogPostModel>
    {
        public Expression<Func<BlogPost, bool>> Criteria => x => !x.Deleted;

        public IEnumerable<string> Includes => new[] { nameof(BlogPost.Comments) };

        public Expression<Func<BlogPost, BlogPostModel>> Projection => post => new BlogPostModel
        {
            Comments = post.Comments.Count,
            Content = post.Content,
            Id = post.Id,
            Likes = post.Likes,
            Title = post.Title
        };

        public IEnumerable<Sort<BlogPost>> SortingInstructions
            => new[] { new Sort<BlogPost> { KeySelector = post => post.PublishingDate, SortingDirection = SortingDirection.Ascending } };
    }

    public interface ISpecification<TEntity, TModel>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        IEnumerable<string> Includes { get; }
        Expression<Func<TEntity, TModel>> Projection { get; }
        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }
    }


    public interface IOperation<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }
        Action<TEntity> Mutation { get; }
        Func<TEntity, bool> Validation { get; }
    }

    public class AddLikeOperation : IOperation<BlogPost>
    {
        private readonly int _id;

        public AddLikeOperation(int id)
        {
            _id = id;
        }

        public Expression<Func<BlogPost, bool>> Criteria => x => x.RowKey == _id.ToString() && x.PartitionKey == Program.PartitionKey;

        public IEnumerable<Sort<BlogPost>>? SortingInstructions => null;

        public Action<BlogPost> Mutation => post => post.Likes++;

        public Func<BlogPost, bool> Validation => post => true;
    }

    public struct Sort<TEntity>
    {
        public SortingDirection SortingDirection { get; set; }
        public Expression<Func<TEntity, object>> KeySelector { get; set; }
    }

    public enum SortingDirection
    {
        Ascending,
        Descending
    }

    public interface IReadRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        Task<TModel> GetAsync<TModel>(ISpecification<TEntity, TModel> specification);
        Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TEntity, TModel> specification);
    }

    public class ReadRepository<TEntity> : IReadRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        private readonly CloudTableClient _client;

        public ReadRepository(CloudStorageAccount cloudStorageAccount)
        {
            _client = cloudStorageAccount.CreateCloudTableClient();
        }

        public async Task<TModel> GetAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            var table = await GetTable();

            var query = table.CreateQuery<TEntity>()
                .Where(specification.Criteria);

            if (specification.SortingInstructions != null)
            {
                query = ApplySorting(query, specification);
            }

            var projection = query.Select(specification.Projection);

            // async?
            return projection.FirstOrDefault();
        }

        public async Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            var table = await GetTable();

            var query = table.CreateQuery<TEntity>()
                .Where(specification.Criteria);

            if (specification.SortingInstructions != null)
            {
                query = ApplySorting(query, specification);
            }

            var projection = query.Select(specification.Projection);

            // async?
            return projection.ToList();
        }

        private async Task<CloudTable> GetTable()
        {
            var table = _client.GetTableReference(typeof(TEntity).Name);

            await table.CreateIfNotExistsAsync();

            return table;
        }

        private IQueryable<TEntity> ApplySorting<TModel>(IQueryable<TEntity> query, ISpecification<TEntity, TModel> specification)
        {
            if (!specification.SortingInstructions.Any())
            {
                return query;
            }

            var firstKeySelector = specification.SortingInstructions.First();
            var orderedQuery = firstKeySelector.SortingDirection == SortingDirection.Ascending
                ? query.OrderBy(firstKeySelector.KeySelector)
                : query.OrderByDescending(firstKeySelector.KeySelector);

            return specification.SortingInstructions
                .Skip(1)
                .Aggregate(
                    orderedQuery,
                    (aggregateOrderedQuery, keySelector) => keySelector.SortingDirection == SortingDirection.Ascending
                        ? aggregateOrderedQuery.ThenBy(keySelector.KeySelector)
                        : aggregateOrderedQuery.ThenByDescending(keySelector.KeySelector));
        }
    }

    public interface IWriteRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        Task CreateEntityAsync(TEntity entity);
        Task UpdateSingleEntityAsync(IOperation<TEntity> operation);
        Task UpdateMultipleEntitiesAsync(IOperation<TEntity> operation);
        Task DeleteEntityAsync(int id);
    }

    public interface ICommandQueuer
    {
        Task Queue(ICommand command);
    }

    public interface ICommand
    {
        Guid CommandId { get; }
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

    public class LikeBlogPostCommand : ICommand
    {
        public Guid CommandId { get; } = Guid.NewGuid();

        public int BlogPostId { get; set; }
    }

    public class LikeBlogPostCommandHandler : ICommandHandler<LikeBlogPostCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public LikeBlogPostCommandHandler(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public Task HandleAsync(LikeBlogPostCommand command)
        {
            throw new NotImplementedException();
        }
    }

    public class LikeBlogPostCommandQueuer : ICommandHandler<LikeBlogPostCommand>
    {
        private readonly ICommandQueuer _commandQueuer;

        public LikeBlogPostCommandQueuer(ICommandQueuer commandQueuer)
        {
            _commandQueuer = commandQueuer;
        }

        public async Task HandleAsync(LikeBlogPostCommand command)
        {
            await _commandQueuer.Queue(command);
        }
    }

    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        private readonly CloudTableClient _client;

        public WriteRepository(CloudStorageAccount cloudStorageAccount)
        {
            _client = cloudStorageAccount.CreateCloudTableClient();
        }

        public async Task CreateEntityAsync(TEntity entity)
        {
            var table = await GetTable();

            var operation = TableOperation.Insert(entity);

            await table.ExecuteAsync(operation);
        }

        public async Task DeleteEntityAsync(int id)
        {
            var table = await GetTable();

            var entity = await GetByIdAsync(id, table);

            var operation = TableOperation.Delete(entity);

            await table.ExecuteAsync(operation);
        }

        public async Task UpdateSingleEntityAsync(IOperation<TEntity> operation)
        {
            var table = await GetTable();

            var entity = table.CreateQuery<TEntity>().Where(operation.Criteria).FirstOrDefault();

            operation.Mutation.Invoke(entity);

            if (operation.Validation.Invoke(entity))
            {
                var replace = TableOperation.Replace(entity);

                await table.ExecuteAsync(replace);
            }
        }

        public async Task UpdateMultipleEntitiesAsync(IOperation<TEntity> operation)
        {
            var table = await GetTable();

            var entities = table.CreateQuery<TEntity>().Where(operation.Criteria).ToList();

            foreach (var entity in entities)
            {
                operation.Mutation.Invoke(entity);
            }

            var replaces = entities.Where(operation.Validation).Select(entity => TableOperation.Replace(entity));

            var batch = new TableBatchOperation();

            foreach (var replace in replaces)
            {
                batch.Append(replace);
            }

            await table.ExecuteBatchAsync(batch);
        }

        private static async Task<TEntity> GetByIdAsync(int id, CloudTable table)
        {
            var retrieveOperation = TableOperation.Retrieve<TEntity>(Program.PartitionKey, id.ToString());

            var result = await table.ExecuteAsync(retrieveOperation);

            var entity = (TEntity)result.Result;
            return entity;
        }

        private async Task<CloudTable> GetTable()
        {
            var table = _client.GetTableReference(typeof(TEntity).Name);

            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
