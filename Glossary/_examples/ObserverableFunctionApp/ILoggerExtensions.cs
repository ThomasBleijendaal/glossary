using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ObserverableFunctionApp
{
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
