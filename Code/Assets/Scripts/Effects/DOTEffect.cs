using UnityEngine;
using System.Collections;

public class DOTEffect : BaseEffect 
{
    public float damage;

    public override void Enter()
    {
        target.ActivateBurn();
    }

    public override void Execute()
    {
        if (target == null) return;

        target.TakeDamage(damage * Time.deltaTime);

        duration -= 1.0f * Time.deltaTime;

        if (duration <= 0.0f)
            Exit();
    }

    public override void Exit()
    {
        target.DeactivateBurn();
        base.Exit();
    }

    public override BaseEffect getEffect()
    {
        return new DOTEffect();
    }

    public override void stack(BaseEffect effect)
    {
        base.stack(effect);
        damage += (effect as DOTEffect).damage * 0.15f;
    }

}
