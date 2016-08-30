using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveSlot : MonoBehaviour 
{
    public string slotName;
    public Image characterImage;

    public Text playerTypeText;
    public Text playerLevelText;
    public Text playerElementText;

    private SaveData data;

	void Start () 
    {
        Init();

        SetInfo();
	}
	
    void Init()
    {
        data = SaveData.LoadPrefs(slotName);
        if (data.isLoadSuccessful == false)
            data.playerType = PlayerType.None;

        if (data.level == string.Empty)
            data.level = PersistentInfo.instance.defaultLevelName;
    }

    void SetInfo()
    {
        characterImage.sprite = PersistentInfo.instance.characterImages[(int)data.playerType];
        playerTypeText.text = "Name: " + data.playerType.ToString();
        playerLevelText.text = "Lvl. " + data.playerLevel.ToString();
        playerElementText.text = "Element: " + data.playerElement.ToString();
    }

    public void SaveInfo()
    {
        if (EntityManager.instance.player == null) return;

        Player player = EntityManager.instance.player;

        data.playerType = player.Playertype;
        data.soulCount = player.SoulCount;
        data.playerElement = player.element;
        data.playerExp = player.Exp.current;
        data.playerMaxExp = player.Exp.max;
        data.playerLevel = player.Level;

        data.weaponCount = InventoryManager.instance.weapons.Count;
        data.potionCount = InventoryManager.instance.potions.Count;

        for (int i = 0; i < data.weaponCount; ++i)
        {
            PlayerPrefs.SetInt(slotName + "Weapon" + i + "ID", InventoryManager.instance.weapons[i].id);
        }

        for (int i = 0; i < data.potionCount; ++i)
        {
            PlayerPrefs.SetInt(slotName + "Potion" + i + "ID", InventoryManager.instance.potions[i].id);
            PlayerPrefs.SetInt(slotName + "Potion" + i + "Amount", InventoryManager.instance.potions[i].amount);
        }

        data.level = Application.loadedLevelName;

        SetInfo();

        SaveData.SavePrefs(slotName, data);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(slotName);
        data = SaveData.LoadPrefs(slotName);
        if (data.isLoadSuccessful == false)
            data.playerType = PlayerType.None;

        SetInfo();
    }
}
