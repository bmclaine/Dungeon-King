using UnityEngine;

public class AttackBoostEffect:BaseEffect
{
    public float atkMulitplier = 1.0f;
    public override BaseEffect getEffect()
    {
        return new AttackBoostEffect();
    }

    public void BoostAtk()
    {
        target.BoostAttack(atkMulitplier);
    }

    public override void Enter()
    {
        BoostAtk();
        Exit();
    }

}

