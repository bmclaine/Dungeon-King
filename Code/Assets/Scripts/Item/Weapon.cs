using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weapon : Item
{
    public AttackInfo damage;
    public BaseEffectObject effect;
    public Element element;
    public GameObject weaponObject;

    public Weapon()
    {
        itemType = ItemType.Weapon;
    }

    public Weapon(Weapon weapon)
    {
        copy(weapon);
    }

    //Use instead of '=' operator
    public override void copy(Item item)
    {
        if (itemType != ItemType.Weapon) return;

        base.copy(item);

        damage = (item as Weapon).damage;
        effect = (item as Weapon).effect;
        weaponObject = (item as Weapon).weaponObject;
    }

    public override Item getItem()
    {
        return new Weapon();
    }
}
