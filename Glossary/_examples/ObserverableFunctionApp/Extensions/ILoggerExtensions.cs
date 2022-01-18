using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ObserverableFunctionApp.Abstractions;

namespace ObserverableFunctionApp.Extensions;

public static class ILoggerExtensions
{
    public static IDisposable BeginHttpContextScope(this ILogger logger, HttpContext httpContext)
    {
        var builder = logger.AddToScope(httpContext.TraceIdentifier);
        
        if (httpContext.Request.Headers.TryGetValue(TraceIdentifierPolicy.TraceIdentifierHeader, out var headerValues) &&
            headerValues.ToString() is string remoteTraceIdentifier)
        {
            builder.AddToScope(remoteTraceIdentifier);
        }

        return builder.BeginScope();
    }

    public static ILogScopeBuilder AddToScope<TValue>(this ILogger logger, TValue value, [CallerArgumentExpression("value")] string argumentExpression = "")
    {
        return new LogScopeBuilder(logger).AddToScope(value, argumentExpression);
    }
}

