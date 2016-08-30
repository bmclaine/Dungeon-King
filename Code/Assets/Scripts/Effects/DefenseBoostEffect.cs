using UnityEngine;

public class DefenseBoostEffect : BaseEffect
{
    [Range(0.0f,5.0f)]
    public float defMultiplier = 0;

    public override BaseEffect getEffect()
    {
        return new DefenseBoostEffect();
    }

    public void BoostDef()
    {
        target.BoostDefense(defMultiplier);
    }

    public override void Enter()
    {
        BoostDef();
    }
}
