using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : BaseMenu
{
    public Selectable firstSelected;
    private Selectable selected;

    [SerializeField]
    private SaveMenu saveMenu;
    [SerializeField]
    private LoadMenu loadMenu;
    [SerializeField]
    private OptionsMenu optionsMenu;
    [SerializeField]
    private ControlsMenu controlsMenu;

    private float delay;

    void Start()
    {
        close();
    }

    void Update()
    {
        if (isActive())
            input();
    }

    public override void input()
    {
    }

    public override void open()
    {
        view.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);

        firstSelected.Select();

        activeMenu = true;
    }

    public override void close()
    {
        view.SetActive(false);
        activeMenu = false;
    }

    public void Resume()
    {
        PauseManager.instance.Resume();
    }

    public void OpenSaveMenu()
    {
        MenuManager.instance.CloseMenu(this);
        MenuManager.instance.OpenMenu(saveMenu);
    }

    public void OpenLoadMenu()
    {
        MenuManager.instance.CloseMenu(this);
        MenuManager.instance.OpenMenu(loadMenu);
    }

    public void OpenOptionsMenu()
    {
        MenuManager.instance.CloseMenu(this);
        MenuManager.instance.OpenMenu(optionsMenu);
    }

    public void OpenControlsMenu()
    {
        MenuManager.instance.CloseMenu(this);
        MenuManager.instance.OpenMenu(controlsMenu);
    }

    public void GoToMainMenu()
    {
        PersistentInfo.LoadLevel("Main Menu");
    }
}
