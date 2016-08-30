using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScene : MonoBehaviour 
{
    [SerializeField]
    private AudioClip winTheme;

    [SerializeField]
    private GameObject[] playerObjects;

    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private Selectable selectable;

	private void Start () 
    {
        if (SoundManager.instance)
            SoundManager.instance.SetBgMusic(winTheme);

        int index = PersistentInfo.selectedPlayer;

        Instantiate(playerObjects[index], createPoint.position, createPoint.rotation);

        if (selectable)
            selectable.Select();
	}

    public void GoToCredits()
    {
        SaveData.SavePrefs(PersistentInfo.slotName, PersistentInfo.saveData);
        PersistentInfo.LoadLevel("Credits");
    }
}
