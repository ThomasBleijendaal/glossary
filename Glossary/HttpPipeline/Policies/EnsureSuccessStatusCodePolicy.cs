﻿using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

internal class EnsureSuccessStatusCodePolicy : IHttpPipelinePolicy
{
    public Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        if (message.Request.EnsureSuccessStatusCode)
        {
            message.Response.HttpResponseMessage.EnsureSuccessStatusCode();
        }

        return next();
    }
}