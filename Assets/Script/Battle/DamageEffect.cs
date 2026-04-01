public class DamageEffect : Effect
{
    float damage;

    public DamageEffect(float damage)
    {
        this.damage = damage;
    }

    public override void Apply(IEffectTarget target)
    {
        target.TakeDamage(damage);
    }
}