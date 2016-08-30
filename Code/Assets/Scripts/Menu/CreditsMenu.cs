using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditsMenu : BaseMenu 
{
    public Selectable selected;

    void Start()
    {
        selected.Select();
    }

    void Update()
    {
        if (isActive())
            input();
    }

    public override void open()
    {
        activeMenu = true;
    }

    public override void close()
    {
        activeMenu = true;
    }

    public override void input()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        PersistentInfo.LoadLevel("Main Menu");
    }
}
