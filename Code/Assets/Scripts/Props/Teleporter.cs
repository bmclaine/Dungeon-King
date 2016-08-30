using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private string levelToLoad;
    [SerializeField]
    private AudioClip teleportSound;

    private void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player == null) return;

        Save();

        PersistentInfo.LoadLevel(levelToLoad);

        if(SoundManager.instance)
            SoundManager.instance.PlayClip(teleportSound, transform.position);
    }

    private void Save()
    {
        SaveData data = new SaveData();
        Player player = EntityManager.instance.player;
        data.playerLevel = player.Level;
        data.playerElement = player.element;
        data.playerExp = player.Exp.current;
        data.playerMaxExp = player.Exp.max;
        data.soulCount = player.SoulCount;
        data.playerType = player.Playertype;

        data.weaponCount = InventoryManager.instance.weapons.Count;
        data.potionCount = InventoryManager.instance.potions.Count;

        for (int i = 0; i < data.weaponCount; ++i)
        {
            PlayerPrefs.SetInt(PersistentInfo.slotName + "Weapon" + i + "ID", InventoryManager.instance.weapons[i].id);
        }

        for (int i = 0; i < data.potionCount; ++i)
        {
            PlayerPrefs.SetInt(PersistentInfo.slotName + "Potion" + i + "ID", InventoryManager.instance.potions[i].id);
            PlayerPrefs.SetInt(PersistentInfo.slotName + "Potion" + i + "Amount", InventoryManager.instance.potions[i].amount);
        }

        PersistentInfo.saveData = data;
    }
}
