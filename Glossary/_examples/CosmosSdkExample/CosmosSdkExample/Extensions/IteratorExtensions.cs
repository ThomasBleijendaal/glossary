using Microsoft.Azure.Cosmos;

namespace CosmosSdkExample.Extensions;

internal static class IteratorExtensions
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this FeedIterator<T> iterator)
    {
        var response = await iterator.ReadNextAsync();

        foreach (var item in response.Resource)
        {
            yield return item;
        }
    }

    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> asyncEnumerable)
    {
        var response = new List<T>();

        await foreach (var item in asyncEnumerable)
        {
            response.Add(item);
        }

        return response;
    }
}
