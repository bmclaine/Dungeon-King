using UnityEngine;

public class AttackBoostEffectObject : BaseEffectObject
{
    public float atkMulitplier = 1.0f;

    public override BaseEffect getEffect()
    {
        AttackBoostEffect newEffect = new AttackBoostEffect();
        newEffect.atkMulitplier = atkMulitplier;
        return newEffect;
    }
}

