using System;
using System.Threading.Tasks;
using HttpPipeline;
using HttpPipeline.Messages;
using Microsoft.AspNetCore.Http;

namespace ObserverableFunctionApp
{
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
            message.Request.SetHeader(TraceIdentifierHeader, _httpContextAccessor.HttpContext.TraceIdentifier);

            return next();
        }
    }
}
