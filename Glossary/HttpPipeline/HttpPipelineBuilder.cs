﻿using HttpPipeline.Policies;

namespace HttpPipeline;

public class HttpPipelineBuilder
{
    public static IHttpPipeline Build(ClientOptions options)
    {
        var policies = new List<IHttpPipelinePolicy>();

        AddPolicies(HttpPipelinePosition.Start);

        if (options.Retry.MaxRetries > 0)
        {
            policies.Add(new RetryPolicy(options.Logger, options.Retry));
        }

        if (options.LogRequests)
        {
            policies.Add(new LogRequestPolicy(options.Logger));
        }

        AddPolicies(HttpPipelinePosition.BeforeTransport);

        policies.Add(new HttpPipelineTransportPolicy(options.Transport));

        if (options.EnableEnsureSuccessStatusCode)
        {
            policies.Add(new EnsureSuccessStatusCodePolicy());
        }

        AddPolicies(HttpPipelinePosition.AfterTransport);

        if (options.LogResponses)
        {
            policies.Add(new LogResponsePolicy(options.Logger));
        }

        if (options.ParseBodyAsJson)
        {
            policies.Add(new ParseBodyAsJsonPolicy());
        }

        AddPolicies(HttpPipelinePosition.End);

        return new HttpPipeline(
            options.RequestBuilder ?? new RequestBuilder(options.BaseUri), 
            policies.ToArray());

        void AddPolicies(HttpPipelinePosition position)
        {
            if (options.Policies != null)
            {
                policies.AddRange(options.Policies.Where(x => x.Position == position).Select(x => x.Policy));
            }
        }
    }
}
