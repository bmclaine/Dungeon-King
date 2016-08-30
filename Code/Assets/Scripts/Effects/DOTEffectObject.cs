using UnityEngine;
using System.Collections;

public class DOTEffectObject : BaseEffectObject
{
    public float damage;

    public override BaseEffect getEffect()
    {
        DOTEffect effect = new DOTEffect();
        effect.damage = damage;
        effect.duration = duration;
        effect.id = id;
        return effect;
    }
}
