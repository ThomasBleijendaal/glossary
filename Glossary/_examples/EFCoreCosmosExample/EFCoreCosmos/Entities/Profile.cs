namespace EFCoreCosmos.Entities;

internal class Profile
{
    public string ProfileId { get; set; } = null!;

    public string PartitionKey { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public Dictionary<string, string> Things { get; set; } = new();
}
