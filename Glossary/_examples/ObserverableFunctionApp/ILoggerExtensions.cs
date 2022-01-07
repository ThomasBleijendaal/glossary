using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ObserverableFunctionApp
{
    // TODO:
    // - convert to adapter
    // - implement LogX<T>, LogX<T,T>, LogX<T,T,T> + IsEnabled
    // - implement BeginScope with dictionary overload

    public static class ILoggerExtensions
    {
        public static IDisposable BeginHttpContextScope(this ILogger logger, HttpContext httpContext)
        {
            return logger.BeginScope(new Dictionary<string, string>
            {
                { nameof(httpContext.TraceIdentifier), httpContext.TraceIdentifier }
            });
        }
    }
}
