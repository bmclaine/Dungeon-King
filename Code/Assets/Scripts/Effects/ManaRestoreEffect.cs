//[ ] Does the mana restore effect class derive from the base effect class?
//[ ] Does the mana restore effect class have a mana amount variable?
class ManaRestoreEffect:BaseEffect
{
    public float mpRestore;
    public override BaseEffect getEffect()
    {
        return new ManaRestoreEffect();
    }

    public void RestoreMana()
    {
        float value = target.Mana.max * mpRestore;
        target.RestoreMana(value);
    }

    public override void Enter()
    {
        RestoreMana();
        Exit();
    }

}

