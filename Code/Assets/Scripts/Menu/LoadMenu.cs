using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadMenu : BaseMenu
{
    [SerializeField]
    private Selectable selected;

    void Start()
    {
    }

    public override void open()
    {
        view.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selected.gameObject);
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
