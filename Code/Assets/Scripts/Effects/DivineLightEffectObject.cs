using UnityEngine;
using System.Collections;

public class DivineLightEffectObject : BaseEffectObject
{
    public override BaseEffect getEffect()
    {
        DivineLightEffect newEffect = new DivineLightEffect();
        newEffect.duration = duration;
        newEffect.chance = chance;
        newEffect.id = id;

        return newEffect;
    }
}
