using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveData 
{
    public PlayerType playerType;
    public Element playerElement;
    public int playerLevel;
    public float playerExp;
    public float playerMaxExp;
    public string level;
    public int elementCount;
    public int soulCount;
    public bool isLoadSuccessful;

    public int potionCount;
    public int weaponCount;

    public SaveData()
    {
        isLoadSuccessful = false;
        level = string.Empty;
        playerLevel = 1;
    }

    public static SaveData Load(string filePath)
    {
        SaveData data = new SaveData();

        if (SaveManager.FileExists(filePath))
        {
            data = (SaveData)SaveManager.Load(data, filePath);
            data.isLoadSuccessful = true;
        }
        else
            data.isLoadSuccessful = false;

        return new SaveData();
    }

    public static SaveData LoadPrefs(string slotName)
    {
        if(!PlayerPrefs.HasKey(slotName))
            return new SaveData();

        SaveData data = new SaveData();
        data.isLoadSuccessful = true;

        //Get EXP
        data.playerExp = PlayerPrefs.GetFloat(slotName + "EXP");
        data.playerMaxExp = PlayerPrefs.GetFloat(slotName + "MaxEXP");

        //Get Level
        data.playerLevel = PlayerPrefs.GetInt(slotName + "PlayerLevel", 1);
        data.level = PlayerPrefs.GetString(slotName + "Level");

        //Get Player Type
        data.playerType = (PlayerType)PlayerPrefs.GetInt(slotName + "Player Type");

        //Get Element
        data.playerElement = (Element)PlayerPrefs.GetInt(slotName + "Player Element");

        //Get Element Count
        data.elementCount = PlayerPrefs.GetInt(slotName + "Element Count");

        //Get Souls & Gold
        data.soulCount = PlayerPrefs.GetInt(slotName + "Souls");

        //Get Item data
        data.potionCount = PlayerPrefs.GetInt(slotName + "PotionCount");
        data.weaponCount = PlayerPrefs.GetInt(slotName + "WeaponCount");

        return data;
    }

    public static void SavePrefs(string slotName, SaveData data)
    {
        if (data == null) return;

        PlayerPrefs.SetString(slotName, slotName);

        //Set EXP
        PlayerPrefs.SetFloat(slotName + "EXP", data.playerExp);
        PlayerPrefs.SetFloat(slotName + "MaxEXP", data.playerMaxExp);

        //Set Level
        PlayerPrefs.SetInt(slotName + "PlayerLevel", data.playerLevel);
        PlayerPrefs.SetString(slotName + "Level", data.level);

        //Set Player Type
        PlayerPrefs.SetInt(slotName + "Player Type", (int)data.playerType);

        //Set Element
        PlayerPrefs.SetInt(slotName + "Player Element", (int)data.playerElement);

        //Set Element Count
        PlayerPrefs.SetInt(slotName + "Element Count", (int)data.elementCount);

        //Set Souls & Gold
        PlayerPrefs.SetInt(slotName + "Souls", data.soulCount);

        //Set Items
        PlayerPrefs.SetInt(slotName + "PotionCount", data.potionCount);
        PlayerPrefs.SetInt(slotName + "WeaponCount", data.weaponCount);
    }
}
