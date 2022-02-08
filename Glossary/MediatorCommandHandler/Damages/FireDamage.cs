namespace MediatorCommandHandler.Damages;

internal record FireDamage(int DamagePoints) : Damage
{
    public override string ToString() => $"{DamagePoints} fire damage";
}
