using UnityEngine;

public class InstantDeathEffectObject : BaseEffectObject
{
    public override BaseEffect getEffect()
    {
        InstantDeathEffect newEffect = new InstantDeathEffect();
        newEffect.chance = chance;
        return newEffect;
    }
}

