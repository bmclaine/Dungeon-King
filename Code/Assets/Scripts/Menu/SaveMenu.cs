using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveMenu : BaseMenu
{
    public Selectable selected;

    void Update()
    {
        if (isActive())
            input();
    }

    public override void open()
    {
        view.SetActive(true);
        selected.Select();
        activeMenu = true;
    }

    public override void close()
    {
        view.SetActive(false);
        activeMenu = false;
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
