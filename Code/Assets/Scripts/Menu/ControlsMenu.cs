using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlsMenu : BaseMenu
{
    [SerializeField]
    private Selectable firstSelected;

    public override void open()
    {
        view.SetActive(true);

        if (firstSelected)
            firstSelected.Select();
    }

    public override void close()
    {
        view.SetActive(false);
    }

    public override void input()
    {
        
    }

    public void OpenPauseMenu()
    {
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();

        MenuManager.instance.CloseMenu(this);
        MenuManager.instance.OpenMenu(pauseMenu);
    }
}
