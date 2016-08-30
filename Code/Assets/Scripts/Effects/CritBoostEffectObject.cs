using UnityEngine;

public class CritBoostEffectObject : BaseEffectObject
{
    [Range(0.0f, 1.0f)]
    public float critMultiplier = 1.0f;

    public override BaseEffect getEffect()
    {
        CritBoostEffect newEffect = new CritBoostEffect();
        newEffect.critMultiplier = critMultiplier;
        return newEffect;
    }
}

