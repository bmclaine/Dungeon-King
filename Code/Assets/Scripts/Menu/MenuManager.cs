using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour 
{
    private static MenuManager manager;
    public static MenuManager instance
    {
        get
        {
            if (manager)
                return manager;

            manager = FindObjectOfType<MenuManager>();

            return manager;
        }
    }

    public BaseMenu startMenu;
    private BaseMenu activeMenu;

    void Start()
    {
        if (startMenu)
            OpenMenu(startMenu);
    }

    public void OpenMenu(BaseMenu menu)
    {
        if (menu)
            menu.open();
    }

    public void CloseMenu(BaseMenu menu)
    {
        if(menu)
            menu.close();
    }

}
