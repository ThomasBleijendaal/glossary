namespace MediatorCommandHandler.Damages;

internal record LightningDamage(int DamagePoints) : Damage
{
    public override string ToString() => $"{DamagePoints} lightning damage";
}
