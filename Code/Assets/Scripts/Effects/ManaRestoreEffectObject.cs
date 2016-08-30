using UnityEngine;

public class ManaRestoreEffectObject : BaseEffectObject
{
    [Range(0.0f,1.0f)]
    public float mpRestore = 1.0f;
    
    public override BaseEffect getEffect()
    {
        ManaRestoreEffect newEffect = new ManaRestoreEffect();
        newEffect.mpRestore = mpRestore;
        return newEffect;
    }
}

