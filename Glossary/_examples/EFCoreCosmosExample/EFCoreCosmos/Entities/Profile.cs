namespace EFCoreCosmos.Entities;

internal class Profile
{
    public string ProfileId { get; set; } = null!;

    public string PartitionKey { get; set; } = null!;

    public string? EmailAddress { get; set; }

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
