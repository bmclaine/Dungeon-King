using UnityEngine;
using System.Collections;

public class FlinchEffectObject : BaseEffectObject
{
    public float flinchModifier;

    public override BaseEffect getEffect()
    {
        FlinchEffect newEffect = new FlinchEffect();
        newEffect.flinchModifier = flinchModifier;
        newEffect.duration = duration;
        newEffect.chance = chance;
        newEffect.id = id;

        return newEffect;
    }
}
