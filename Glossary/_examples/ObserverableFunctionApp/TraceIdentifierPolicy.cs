using System;
using System.Linq;
using System.Threading.Tasks;
using HttpPipeline;
using HttpPipeline.Messages;
using Microsoft.AspNetCore.Http;

namespace ObserverableFunctionApp;

/// <summary>
/// Makes it easy to send the trace to other services for tracing requests.
/// </summary>
public class TraceIdentifierPolicy : IHttpPipelinePolicy
{
    public const string TraceIdentifierHeader = "X-Trace";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public TraceIdentifierPolicy(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        var traceIdentifiers =
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(TraceIdentifierHeader, out var remoteTraceIdentifiers) &&
            remoteTraceIdentifiers.ToArray() is string[] identifiers
            ? identifiers.Append(_httpContextAccessor.HttpContext.TraceIdentifier)
            : new[] { _httpContextAccessor.HttpContext.TraceIdentifier };

        message.Request.SetHeader(TraceIdentifierHeader, string.Join(',', traceIdentifiers));

        return next();
    }
}
