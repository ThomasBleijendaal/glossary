namespace MediatorCommandHandler.Damages;

internal record MeleeDamage(int DamagePoints) : Damage
{
    public override string ToString() => $"{DamagePoints} damage";
}
