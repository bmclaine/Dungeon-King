using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item 
{
    public string name;
    public string info;
    public int id;
    public int amount;
    public Sprite icon;
    public ItemType itemType;

    public Item()
    {
        itemType = ItemType.None;
    }

    public Item(Item item)
    {
        item.copy(item);
    }

    //Use instead of '=' operator
    public virtual void copy(Item item)
    {
        name = item.name;
        info = item.info;
        id = item.id;
        amount = item.amount;
        icon = item.icon;
        itemType = item.itemType;
    }

    public virtual Item getItem()
    {
        return new Item();
    }
}

public enum ItemType
{
    None,
    Item,
    Weapon,
    Potion,
    Gold
}
