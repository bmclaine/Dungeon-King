using UnityEngine;
using System.Collections;

public class FlinchEffect : BaseEffect
{
    public float flinchModifier;

    public override void Enter()
    {
        target.ModifyFlinch(flinchModifier);
    }

    public override void Execute()
    {
        if (target == null) return;

        duration -= Time.deltaTime;

        if (duration <= 0.0f)
            Exit();
    }

    public override void Exit()
    {
        target.ModifyFlinch(-flinchModifier);
        target.RemoveEffect(this);
    }

    public override BaseEffect getEffect()
    {
        return new FlinchEffect();
    }
}
