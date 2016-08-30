using UnityEngine;
using System.Collections;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase database;

    public static ItemDatabase instance
    {
        get
        {
            if (database)
                return database;

            database = FindObjectOfType<ItemDatabase>();
            return database;
        }
    }

    [SerializeField]
    private Potion[] potions;
    [SerializeField]
    private Weapon[] weapons;

    public int PotionCount
    {
        get
        {
            return potions.Length;
        }
    }

    public int WeaponCount
    {
        get
        {
            return weapons.Length;
        }
    }

    public Potion GetPotion(int index)
    {
        return potions[index];
    }

    public Weapon GetWeapon(int index)
    {
        return weapons[index];
    }
}
