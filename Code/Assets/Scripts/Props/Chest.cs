using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    private int potionCount;
    [SerializeField]
    private Weapon weapon;

    public bool isOpen;

    [SerializeField]
    private AudioClip chestOpen;
    [SerializeField]
    private AudioClip chestClose;

    private Animator animator;

    public List<Item> itemList = new List<Item>();
    new Renderer renderer;

    private string id;

    public bool playerInRange;

    void Start()
    {
        //SetID();

        isOpen = false;
        animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<Renderer>();

        init();
    }

    void init()
    {
        potionCount = Random.Range(1, 4);

        for (int i = 0; i < potionCount; i++)
        {
            int index = Random.Range(0, ItemDatabase.instance.PotionCount);
            Potion potion = new Potion();
            potion.copy(ItemDatabase.instance.GetPotion(index));
            itemList.Add(potion);
        }

        if (weapon.itemType == ItemType.Weapon)
        {
            itemList.Add(weapon);
            PlayerPrefs.SetInt(id + "hasWeapon", 1);
        }
    }

    void Load()
    {
        potionCount = PlayerPrefs.GetInt(id + "Item Count", 1);

        for (int i = 0; i < potionCount; i++)
        {
            int index = Random.Range(0, ItemDatabase.instance.PotionCount);
            Potion potion = new Potion();
            potion.copy(ItemDatabase.instance.GetPotion(index));
            itemList.Add(potion);
        }

        int hasWeapon = PlayerPrefs.GetInt(id + "hasWeapon", 1);

        if (hasWeapon == 0) return;
        if (weapon.itemType == ItemType.Weapon)
        {
            itemList.Add(weapon);
            PlayerPrefs.SetInt(id + "hasWeapon", 1);
        }
    }

    void Update()
    {
        input();

        if (itemList.Count == 0)
        {
            Color color = renderer.material.color;
            color.a -= Time.deltaTime * 0.3f;
            renderer.material.color = color;

            if (color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void input()
    {
        if (playerInRange == false || isOpen || itemList.Count <= 0) return;

        if(Input.GetButtonDown("Interact"))
        {
            isOpen = true;
            animator.SetBool("isOpen", isOpen);
            PlayChestOpen();

            if (!HUDInterface.instance) return;

            HUDInterface.instance.chest = this;
            HUDInterface.instance.ResetLoot();
        }
    }   

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (!player) return;

        playerInRange = true;
    }

    void OnTriggerExit(Collider col)
    {
        if (!isOpen) return;

        Player player = col.GetComponent<Player>();

        if (player == null) return;

        playerInRange = false;
        isOpen = false;
        animator.SetBool("isOpen", isOpen);

        PlayChestClose();

        if (!HUDInterface.instance) return;

        HUDInterface.instance.ClearLoot();
        HUDInterface.instance.chest = null;
    }

    public void RemoveItem(int index)
    {
        if (!InventoryManager.instance) return;

        if(itemList[index].itemType == ItemType.Weapon)
        {
            PlayerPrefs.SetInt(id + "hasWeapon", 0);
        }

        InventoryManager.instance.AddItem(itemList[index]);
        itemList.RemoveAt(index);

        Save();

        HUDInterface.instance.ResetLoot();
    }

    private void PlayChestOpen()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(chestOpen, Vector3.zero);
    }

    private void PlayChestClose()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(chestClose, Vector3.zero);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(id + "Item Count", itemList.Count);

        PlayerPrefs.SetString(id, id);
    }

    private void SetID()
    {
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        int z = (int)transform.position.z;

        id = PersistentInfo.slotName + (x.ToString() + y.ToString() + z.ToString());
    }
}
