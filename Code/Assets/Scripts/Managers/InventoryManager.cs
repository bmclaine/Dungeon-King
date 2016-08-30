using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager manager;

    public static InventoryManager instance
    {
        get
        {
            if (manager)
                return manager;

            manager = FindObjectOfType<InventoryManager>();

            return manager;
        }
    }

    public List<Potion> potions = new List<Potion>();
    public List<Weapon> weapons = new List<Weapon>();
    [SerializeField]
    private GameObject currentWeaponObj;
    [SerializeField]
    private Transform weaponTransform;

    private int potionIndex;
    private int weaponIndex;

    private float delay;

    [SerializeField]
    private AudioClip weaponCycle;
    [SerializeField]
    private AudioClip potionCycle;
    [SerializeField]
    private AudioClip potionDrink;

    public Weapon currentWeapon
    {
        get
        {
            if (weapons.Count == 0)
                return new Weapon();

            return weapons[weaponIndex];
        }
    }

    void Start()
    {
        HUDInterface.instance.SetWeaponQuickslot(weapons[0]);

        GameObject weapongo = GameObject.FindGameObjectWithTag("Weapon");

        if (weapongo)
            weaponTransform = weapongo.transform;

        Load();
    }

    void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        input();
    }

    private void input()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CyclePotion(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            CycleWeapon(1);

        if (Input.GetButtonDown("PotionUse"))
            UsePotion();

        float h = Input.GetAxisRaw("DPadX");
        float v = Input.GetAxis("DPadY");

        if(h != 0.0f && delay <= 0.0f)
        {
            delay = 0.2f;
            int H = Mathf.RoundToInt(h);
            CyclePotion(H);
        }

        if(v != 0.0f && delay <= 0.0f)
        {
            delay = 0.2f;
            int V = Mathf.RoundToInt(v);
            CycleWeapon(V);
        }

        delay -= Time.deltaTime;
    }

    private void AddPotion(Potion potion)
    {
        if (potion.itemType != ItemType.Potion) return;

        Potion temp = potions.Find(x => x.id == potion.id);

        if (temp != null)
            temp.amount++;
        else
            potions.Add(potion);

        for (int i = 0; i < potions.Count; i++)
        {
            if (potions[i].amount > 9)
            {
                potions[i].amount--;
                CyclePotion(0);
            }
        }

        CyclePotion(0);
    }

    public void UsePotion()
    {
        if (potions.Count == 0)
            return;

        BaseEffectObject effect = potions[potionIndex].effect;

        EntityManager.instance.player.AddEffect(effect);

        if (SoundManager.instance)
            SoundManager.instance.PlayClip(potionDrink, Vector3.zero);

        potions[potionIndex].amount--;

        if (potions[potionIndex].amount == 0)
        {
            potions.RemoveAt(potionIndex);
        }

        CyclePotion(0);
    }

    public void CyclePotion(int value)
    {
        if (potions.Count == 0)
        {
            if (HUDInterface.instance)
                HUDInterface.instance.ResetPotionQuickslot();

            return;
        }

        potionIndex += value;

        if (potionIndex >= potions.Count)
            potionIndex = 0;

        if (potionIndex < 0)
            potionIndex = potions.Count - 1;

        if (HUDInterface.instance)
            HUDInterface.instance.SetPotionQuickslot(potions[potionIndex]);

        if (SoundManager.instance)
            SoundManager.instance.PlayClip(potionCycle, Vector3.zero);
    }

    private void AddWeapon(Weapon weapon)
    {
        if (weapon.itemType != ItemType.Weapon) return;

        weapons.Add(weapon);

        CycleWeapon(0);
    }

    public void CycleWeapon(int value)
    {
        weaponIndex += value;

        if (weaponIndex >= weapons.Count)
            weaponIndex = 0;

        if (weaponIndex < 0)
            weaponIndex = weapons.Count - 1;

        if (HUDInterface.instance)
            HUDInterface.instance.SetWeaponQuickslot(weapons[weaponIndex]);


        if (SoundManager.instance)
            SoundManager.instance.PlayClip(weaponCycle, Vector3.zero);

        if (currentWeaponObj != null)
        {
            currentWeaponObj.gameObject.SetActive(false);
        }

        if (!weaponTransform)
        {
            GameObject weapongo = GameObject.FindGameObjectWithTag("Weapon");

            if (weapongo)
                weaponTransform = weapongo.transform;
        }

        if (!weaponTransform) return;

        foreach (Transform weapon in weaponTransform)
        {
            if(!currentWeaponObj)
            {
                if (weapon.gameObject.activeSelf)
                    weapon.gameObject.SetActive(false);
            }

            if (weapons[weaponIndex].weaponObject.name == weapon.name)
            {
                currentWeaponObj = weapon.gameObject;
                weapon.gameObject.SetActive(true);
            }
        }
    }

    public void AddItem(Item item)
    {
        if (item.itemType == ItemType.Weapon) AddWeapon(item as Weapon);

        if (item.itemType == ItemType.Potion) AddPotion(item as Potion);
    }

    private void Load()
    {
        if (PersistentInfo.saveData == null) return;
       
        int weaponCount = PersistentInfo.saveData.weaponCount;
        int potionCount = PersistentInfo.saveData.potionCount;

        string slotName = PersistentInfo.slotName;

        for(int i = 1; i < weaponCount; ++i)
        {
            int weaponId = PlayerPrefs.GetInt(slotName + "Weapon" + i + "ID", -1);

            if (weaponId == -1) continue;
            
            Weapon weapon = new Weapon();
            weapon.copy(ItemDatabase.instance.GetWeapon(weaponId));
            AddWeapon(weapon);
        }

        for(int i = 0; i < potionCount; ++i)
        {
            int potionId = PlayerPrefs.GetInt(slotName + "Potion" + i + "ID", -1);
            
            if (potionId == -1) continue;

            Potion potion = new Potion();
            potion.copy(ItemDatabase.instance.GetPotion(potionId));

            int amount = PlayerPrefs.GetInt(slotName + "Potion" + i + "Amount");
            potion.amount = amount;

            AddPotion(potion);
        }
    }
}
