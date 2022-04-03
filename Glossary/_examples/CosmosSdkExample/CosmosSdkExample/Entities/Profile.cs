using Newtonsoft.Json;

namespace CosmosSdkExample.Entities;

internal class Profile
{
    [JsonProperty("id")]
    public string ProfileId { get; set; } = null!;

    public string PartitionKey { get; set; } = null!;

    public string? EmailAddress { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public List<Preference> Things { get; set; } = new();
}

internal abstract class Preference
{
    public string Key { get; set; } = null!;
}

internal class StringPreference : Preference
{
    public string Setting { get; set; } = null!;
}

internal class IntegerPreference : Preference
{
    public int Setting { get; set; }
}
