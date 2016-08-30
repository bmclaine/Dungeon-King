using UnityEngine;
using System.Collections;

public class BaseEffectObject : ScriptableObject
{
    public int id;
    public float duration;

    [Range(0,1)]
    public float chance;

    public virtual BaseEffect getEffect()
    {
        BaseEffect newEffect = new BaseEffect();
        newEffect.duration = duration;
        newEffect.chance = chance;
        newEffect.id = id;

        return newEffect;
    }
    
}
