using UnityEngine;

public class HealthRestoreEffectObject : BaseEffectObject
{
    [Range(0.0f,1.0f)]
    public float percent;

    public override BaseEffect getEffect()
    {
        HealthRestoreEffect effect = new HealthRestoreEffect();
        effect.id = id;
        effect.percent = percent;
        effect.duration = duration;
        return effect;
    }
}

