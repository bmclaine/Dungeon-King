using UnityEngine;

public class InstantDeathEffect:BaseEffect
{
    public override BaseEffect getEffect()
    {
        return new InstantDeathEffect();
    }

    public void InstantDeath()
    {
        float damage = target.Health.max + target.Health.current;
        target.TakeDamage(damage);
    }

    public override void Enter()
    {
        InstantDeath();
    }
}

