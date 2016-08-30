using UnityEngine;
using System.Collections;

public class DivineLightEffect : BaseEffect
{
    public override void Enter()
    {
        (target as Player).ToggleDivineLight(true);
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
        (target as Player).ToggleDivineLight(false);
        base.Exit();
    }

    public override BaseEffect getEffect()
    {
        return new DivineLightEffect();
    }
}
