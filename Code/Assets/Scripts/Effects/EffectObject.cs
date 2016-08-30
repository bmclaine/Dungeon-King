using UnityEngine;
using System.Collections;

public class EffectObject : MonoBehaviour
{
    private Entity target;
    public Entity Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    public BaseEffectObject effect;

    public void ActivateEffect()
    {
        if (target)
            target.AddEffect(effect);

        Destroy(this.gameObject);
    }
}
