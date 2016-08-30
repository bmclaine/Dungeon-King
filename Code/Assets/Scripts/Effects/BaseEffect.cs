using UnityEngine;
using System.Collections;

public class BaseEffect
{
    [Range(0.0f,1.0f)]
    public float chance;
    public float duration;
    public int id;
    protected Entity target;

    public virtual void Enter()
    {

    }

    public virtual void Execute()
    {
        if (target == null) return;
    }

    public virtual void Exit()
    {
        target.RemoveEffect(this);
    }

    public virtual BaseEffect getEffect()
    {
        return new BaseEffect();
    }

    public virtual void stack(BaseEffect effect)
    {
        duration += effect.duration;
    }

    public override bool Equals(object obj)
    {
        BaseEffect effect = (BaseEffect)obj;
        if (effect == null)
            return false;

        return id == effect.id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public void SetTarget(Entity _target)
    {
        target = _target;
    }
}
