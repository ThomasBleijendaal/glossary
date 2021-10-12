using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Dispatchers;
using AzureCommandDispatcher.Services.Handlers;
using AzureCommandDispatcher.Services.Models.Commands;
using AzureCommandDispatcher.Services.Models.Responses;
using AzureCommandDispatcher.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AzureCommandDispatcher.Services;

public static class DependencyConfiguration
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<AddCommand, ResultResponse>, AddCommandHandler>();
        services.AddTransient<ICommandHandler<MultiplyCommand, ResultResponse>, AsyncCommandHandler<MultiplyCommand, ResultResponse>>();

        services.AddSingleton<IAsyncDispatcher, AsyncDispatcher>();
        services.AddSingleton<IDeferredResponseService, DeferredResponseService>();
    }
}
