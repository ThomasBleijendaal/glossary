﻿/**
* Command and Query Responsibility Separation aims for separating getting data from storing data
* from a data store. Because the read and write responsibilities are handled by different classes
* and objects, the data store responsible for giving data and storing data can also be different.
* It's possible to store data in a sql server, and read data from a document store, or use EF for
* CUD actions and use Dapper for reading, or vice versa.
* 
* The read repository only uses specifications to fetch data, either one entity or a list. The read repository
* could forward these specifications to a cache service for local caching, or a memory store nearby.
* 
* The write repository supports creation, mutation and deletion. These operations are specified by the
* service wrapping the repository, and are first posted to a command handler. That handler can either
* directly forward it to the repository, or post it to a queue and have it processed out-of-process. That
* out-of-process processing can refresh or revoke the cache that the read repository uses.
*/
    
using System;
using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Handlers;
using CQRS.Repositories;
using CQRS.Services;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ZCommon;

namespace CQRS
{
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

            services.AddTransient(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddTransient(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            services.AddTransient<IBlogPostService, BlogPostService>();
            services.AddTransient<ICommentService, CommentService>();

            services.AddTransient<ICommandQueuingService, CommandQueuingService>();

            // These command handlers directly talk to the database, and execute the commands as they come in
            services.AddTransient<ICommandHandler<CreateBlogPostCommand>, CreateBlogPostCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteBlogPostCommand>, DeleteBlogPostCommandHandler>();

            // These command handlers post their commands to a queue, to allow for out-of-process handing
            services.AddTransient<ICommandHandler<AddCommentCommand>, QueueCommandHandler<AddCommentCommand>>();
            services.AddTransient<ICommandHandler<AddLikeCommand>, QueueCommandHandler<AddLikeCommand>>();
        }

        public class RepositoryCQRS : BaseApp
        {
            private readonly IBlogPostService _blogPostService;
            private readonly ICommentService _commentService;

            public RepositoryCQRS(IBlogPostService blogPostService, ICommentService commentService)
            {
                _blogPostService = blogPostService;
                _commentService = commentService;
            }

            public override async Task Run()
            {
                var blogPostId = await _blogPostService.AddBlogPostAsync("New post!", "Hello world.");

                await _commentService.AddCommentAsync(blogPostId, "Commenter 1", "Nice post.");
                await _commentService.AddCommentAsync(blogPostId, "Commenter 2", "Nice post!");

                // this instruction will not work normally since some of these methods will try to update
                // an entity that has changed in the mean time, failing the ETag validation
                await Task.WhenAll(
                    _commentService.AddLikeAsync(blogPostId),
                    _commentService.AddLikeAsync(blogPostId),
                    _commentService.AddLikeAsync(blogPostId),
                    _commentService.AddLikeAsync(blogPostId),
                    _commentService.AddLikeAsync(blogPostId));

                var blogPosts = await _blogPostService.GetBlogPostsAsync();

                Console.WriteLine(JsonConvert.SerializeObject(blogPosts));

                await Task.Delay(30000);

                blogPosts = await _blogPostService.GetBlogPostsAsync();

                Console.WriteLine(JsonConvert.SerializeObject(blogPosts));

                await _blogPostService.DeleteBlogPostAsync(blogPostId);
            }
        }
    }
}
