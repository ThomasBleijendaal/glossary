namespace MediatorCommandHandler.Damages;

internal record AirDamage(int DamagePoints) : Damage
{
    public override string ToString() => $"{DamagePoints} air damage";
}
