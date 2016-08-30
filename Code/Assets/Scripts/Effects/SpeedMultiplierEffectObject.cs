using UnityEngine;
using System.Collections;

public class SpeedMultiplierEffectObject : BaseEffectObject
{
    public float mulitiplier;

    public override BaseEffect getEffect()
    {
        SpeedMultiplierEffect effect = new SpeedMultiplierEffect();
        effect.multiplier = mulitiplier;
        effect.duration = duration;
        effect.id = id;
        return effect;
    }  
}
