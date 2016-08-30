using UnityEngine;
using System.Collections;

public abstract class BaseMenu : MonoBehaviour 
{
    public GameObject view;
    public bool activeMenu = false;

    public abstract void open();
    public abstract void close();
    public abstract void input();

    public bool isActive()
    {
        return activeMenu == true;
    }
}
