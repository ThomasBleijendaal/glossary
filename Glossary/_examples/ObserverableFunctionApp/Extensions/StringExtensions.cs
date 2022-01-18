using System.Diagnostics.CodeAnalysis;

namespace ObserverableFunctionApp.Extensions;

public static class StringExtensions
{
    [return: NotNullIfNotNull("text")]
    public static string? ToFirstLetterLower(this string? text)
        => string.IsNullOrEmpty(text) || text.Length < 2
            ? text?.ToLower()
            : $"{text[..1].ToLower()}{text[1..]}";
}
