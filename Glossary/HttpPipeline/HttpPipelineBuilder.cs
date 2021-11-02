using HttpPipeline.Policies;

namespace HttpPipeline;

public class HttpPipelineBuilder
{
    public static IHttpPipeline Build(ClientOptions options)
    {
        var policies = new List<IHttpPipelinePolicy>();

        AddPolicies(HttpPipelinePosition.PerCall);

        if (options.LogRequests)
        {
            policies.Add(new LogRequestPolicy(options.Logger));
        }

        if (options.Retry.MaxRetries > 0)
        {
            policies.Add(new RetryPolicy(options.Logger, options.Retry));
        }

        AddPolicies(HttpPipelinePosition.PerRetry);

        if (options.LogResponses)
        {
            policies.Add(new LogResponsePolicy(options.Logger));
        }

        if (options.ParseBodyAsJson)
        {
            policies.Add(new ParseBodyAsJsonPolicy());
        }

        policies.Add(new EnsureSuccessStatusCodePolicy());
        
        policies.Add(new HttpPipelineTransportPolicy(options.Transport));

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
