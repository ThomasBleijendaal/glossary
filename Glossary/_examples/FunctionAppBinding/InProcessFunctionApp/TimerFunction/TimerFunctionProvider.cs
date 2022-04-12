using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Script.Description;
using Newtonsoft.Json.Linq;

namespace InProcessFunctionApp.TimerFunction;

internal class TimerFunctionProvider : IFunctionProvider
{
    public ImmutableDictionary<string, ImmutableArray<string>> FunctionErrors
        => new Dictionary<string, ImmutableArray<string>>().ToImmutableDictionary();

    public Task<ImmutableArray<FunctionMetadata>> GetFunctionMetadataAsync()
    {
        var list = new List<FunctionMetadata>
        {
            GetDurableMediatorFunctionMetadata("A", "*/2 * * * * *"),
            GetDurableMediatorFunctionMetadata("B", "*/4 * * * * *"),
            GetDurableMediatorFunctionMetadata("C", "*/8 * * * * *"),
            GetDurableMediatorFunctionMetadata("D", "*/16 * * * * *")
        };

        return Task.FromResult(list.ToImmutableArray());
    }

    private FunctionMetadata GetDurableMediatorFunctionMetadata(string name, string pattern)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var functionMetadata = new FunctionMetadata()
        {
            Name = name,
            FunctionDirectory = null,
            ScriptFile = $"assembly:{assembly.FullName}",
            EntryPoint = $"{assembly.GetName().Name}.TimerFunctionProvider.{nameof(TimerFunction)}.{nameof(TimerFunction.Run)}",
            Language = "DotNetAssembly"
        };

        functionMetadata.Bindings.Add(
            BindingMetadata.Create(
                JObject.FromObject(new TimerTriggerMetadata(pattern))));

        return functionMetadata;
    }
}
