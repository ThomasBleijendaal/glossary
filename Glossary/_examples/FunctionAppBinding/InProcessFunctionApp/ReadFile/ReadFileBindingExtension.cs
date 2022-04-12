using System;
using Microsoft.Azure.WebJobs;

namespace InProcessFunctionApp.ReadFile;

internal static class ReadFileBindingExtension
{
    public static IWebJobsBuilder AddReadFileBinding(this IWebJobsBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.AddExtension<ReadFileExtension>();
        return builder;
    }
}

