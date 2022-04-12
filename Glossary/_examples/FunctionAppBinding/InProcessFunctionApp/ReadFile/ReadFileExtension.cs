using System.IO;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

namespace InProcessFunctionApp.ReadFile;

[Extension(nameof(ReadFileExtension))]
internal class ReadFileExtension : IExtensionConfigProvider
{
    public void Initialize(ExtensionConfigContext context)
    {
        var rule = context.AddBindingRule<ReadFileAttribute>();
        rule.BindToInput(async (ReadFileAttribute attribute, ValueBindingContext context) =>
        {

            var fileContext = await File.ReadAllTextAsync(attribute.FileName);

            return fileContext;
        });
    }
}

