using UnityEngine;

public class CritBoostEffect : BaseEffect
{
    public float critMultiplier = 1.0f;

    public override BaseEffect getEffect()
    {
        return new CritBoostEffect();
    }

    public void IncreaseCritChance()
    {
        target.BoostCritical(critMultiplier);
    }

    public override void Enter()
    {
        IncreaseCritChance();
        Exit();
    }
}

