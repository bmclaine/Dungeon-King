using UnityEngine;
using System.Collections;

public class SpeedMultiplierEffect : BaseEffect
{
    public float multiplier;

    public override void Enter()
    {
        target.ModifyMoveSpeed(multiplier);
    }

    public override void Execute()
    {
        if (target == null) return;

        duration -= 1.0f * Time.deltaTime;

        if (duration <= 0.0f)
            Exit();
    }

    public override void Exit()
    {
        target.ModifyMoveSpeed(-multiplier);
        base.Exit();
    }

    public override BaseEffect getEffect()
    {
        return new SpeedMultiplierEffect();
    }
}
