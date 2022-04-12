using System;
using Microsoft.Azure.WebJobs.Description;

namespace InProcessFunctionApp.ReadFile;

[Binding]
[AttributeUsage(AttributeTargets.Parameter)]
internal class ReadFileAttribute : Attribute
{
    public ReadFileAttribute(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; }
}

