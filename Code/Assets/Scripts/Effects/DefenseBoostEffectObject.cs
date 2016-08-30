using UnityEngine;

public class DefenseBoostEffectObject : BaseEffectObject
{
    public float defMulitplier = 1.0f;

    public override BaseEffect getEffect()
    {
        DefenseBoostEffect newEffect = new DefenseBoostEffect();
        newEffect.defMultiplier = defMulitplier;
        return newEffect;
    }
}

