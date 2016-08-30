using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadSlot : MonoBehaviour 
{
    public string slotName;
    public Image characterImage;

    public Text playerTypeText;
    public Text playerLevelText;
    public Text playerElementText;

    private SaveData data;

	void Start () 
    {
        init();

        SetInfo();
	}

    void init()
    {
        data = SaveData.LoadPrefs(slotName);
        if (data.isLoadSuccessful == false)
            data.playerType = PlayerType.None;
    }

    void SetInfo()
    {
        playerTypeText.text = "Name: " + data.playerType.ToString();
        playerLevelText.text = "Lvl. " + data.playerLevel.ToString();
        playerElementText.text = "Element: " + data.playerElement.ToString();
        if(characterImage)
            characterImage.sprite = PersistentInfo.instance.characterImages[(int)data.playerType];
    }

    public void SetInfo(SaveData _data)
    {
        data = _data;

        if (data.isLoadSuccessful == false)
            data.playerType = PlayerType.None;

        if (data.level == string.Empty)
            data.level = PersistentInfo.instance.defaultLevelName;   

        SetInfo();
    }

    public void SelectSlot()
    {
        if (data.isLoadSuccessful == false)
            return;

        PersistentInfo.slotName = slotName;
        PersistentInfo.saveData = data;
        Application.LoadLevel(data.level);
    }

    void OnEnabled()
    {
        data = SaveData.Load(slotName);

        if (data.isLoadSuccessful == false)
            data.playerType = PlayerType.None;

        SetInfo();
    }
}
