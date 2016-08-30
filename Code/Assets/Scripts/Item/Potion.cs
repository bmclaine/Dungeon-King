using UnityEngine;
using System.Collections;

[System.Serializable]
public class Potion : Item
{
    public BaseEffectObject effect;


    public Potion()
    {
        itemType = ItemType.Potion;
    }

    public Potion(Potion potion)
    {
        copy(potion);
    }

    //Use instead of '=' operator
    public override void copy(Item item)
    {
        if (itemType != ItemType.Potion) return;

        base.copy(item);

        effect = (item as Potion).effect;
    }

    public override Item getItem()
    {
        return new Potion();
    }
}
