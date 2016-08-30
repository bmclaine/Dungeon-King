
using UnityEngine;

public class HealthRestoreEffect : BaseEffect
{
    [Range(0.0f,1.0f)]
    public float percent;

    public override void Enter()
    {
        float amount = target.Health.max * percent;
        target.RestoreHealth(amount);

        Exit();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override BaseEffect getEffect()
    {
        return new HealthRestoreEffect();
    }
}
