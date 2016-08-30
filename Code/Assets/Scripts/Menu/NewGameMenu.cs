using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NewGameMenu : BaseMenu
{
    [SerializeField]
    private Selectable selectedOption;
    [SerializeField]

    void Start()
    {
        if (selectedOption)
            selectedOption.Select();

        OptionsData data = OptionsData.Load();
        Screen.fullScreen = data.fullscreen;
    }

    void Update()
    {
        input();

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(selectedOption.gameObject);
    }

    public override void open()
    {
        view.SetActive(true);
        activeMenu = true;
    }

    public override void close()
    {
        view.SetActive(false);
        activeMenu = false;
    }

    public override void input()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            LoadLevel("Character Select");
        }
    }

    public void LoadLevel(string level)
    {
        PersistentInfo.LoadLevel(level);
    }
}
