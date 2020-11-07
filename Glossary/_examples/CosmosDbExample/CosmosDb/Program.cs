using System;
using System.Linq;
using System.Threading.Tasks;
using CosmosDb.Repositories.Abstractions.Repositories;
using CosmosDb.Repositories.Repositories;
using CosmosDb.Services.Abstractions;
using CosmosDb.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CosmosDb
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<App>();

            services.AddSingleton(new MongoClient("mongodb://localhost:C2y6yDjf5%2FR%2Bob0N8A7Cgv30VRDJIWEHLM%2B4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw%2FJw%3D%3D@localhost:10255/admin?ssl=true"));
            services.AddSingleton(sp =>
            {
                return sp.GetRequiredService<MongoClient>().GetDatabase("example");
            });

            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var app = scope.ServiceProvider.GetRequiredService<App>();

            await app.RunAsync();

            Console.ReadLine();
        }

        public class App
        {
            private readonly IBlogPostService _blogPostService;

            public App(IBlogPostService blogPostService)
            {
                _blogPostService = blogPostService;
            }

            public async Task RunAsync()
            {
                var postId = await _blogPostService.CreateBlogPostAsync("My blog post!", "Awesome");

                var rng = new Random();
                var comments = rng.Next(1, 3);
                var likes = rng.Next(10, 30);

                for (var i = 0; i < comments; i++)
                {
                    await _blogPostService.CommentBlogPostAsync(postId, Guid.NewGuid().ToString());
                }

                for (var i = 0; i < likes; i++)
                {
                    await _blogPostService.LikeBlogPostAsync(postId);
                }

                var posts = await _blogPostService.GetTrendingBlogPostsAsync();

                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.Title} - Comments: {string.Join(',', post.Comments.Select(x => x.Content))}, Likes: {post.Likes}.");
                }
            }
        }
    }
}
