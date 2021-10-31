﻿using HttpPipeline.Messages;
using Newtonsoft.Json;

namespace HttpPipeline.Policies;

public class ParseBodyAsJsonPolicy : IHttpPipelinePolicy
{
    public async Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        if (message.Request.ResponseType is Type responseType)
        {
            var responseObject = JsonConvert.DeserializeObject(
                await message.Response.HttpResponseMessage.Content.ReadAsStringAsync(),
                responseType);

            message.Response.Content = responseObject;
        }
        await next();
    }
}