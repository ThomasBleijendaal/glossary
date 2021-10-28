using HttpPipeline.Policies;

namespace HttpPipeline;

public class HttpPipelineBuilder
{
    public static HttpPipeline Build(ClientOptions options)
    {
        var policies = new List<HttpPipelinePolicy>();

        AddPolicies(HttpPipelinePosition.Start);

        if (options.LogRequests)
        {
            policies.Add(new LogRequestPolicy(options.Logger));
        }

        AddPolicies(HttpPipelinePosition.BeforeHttpClient);

        policies.Add(new HttpPipelineClientPolicy(options.HttpClientFactory));

        AddPolicies(HttpPipelinePosition.AfterHttpClient);

        if (options.LogResponses)
        {
            policies.Add(new LogResponsePolicy(options.Logger));
        }

        AddPolicies(HttpPipelinePosition.End);

        return new HttpPipeline(policies.ToArray());

        void AddPolicies(HttpPipelinePosition position)
        {
            if (options.Policies != null)
            {
                policies.AddRange(options.Policies.Where(x => x.Position == position).Select(x => x.Policy));
            }
        }
    }
}
