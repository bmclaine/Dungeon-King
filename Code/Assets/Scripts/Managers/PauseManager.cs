using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    private static PauseManager manager;
    public PauseState state;
    public static PauseManager instance
    {
        get
        {
            if (manager)
                return manager;

            manager = FindObjectOfType<PauseManager>();

            if (manager == null)
            {
                GameObject go = new GameObject();
                go.name = "Pause Manager";
                go.AddComponent<PauseManager>();
                manager = go.GetComponent<PauseManager>();
            }

            return manager;
        }
    }

    private PauseMenu pauseMenu;

    void Start()
    {
        state = PauseState.Play;
        pauseMenu = FindObjectOfType<PauseMenu>();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        TogglePause();
    }

    void TogglePause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameManager.instance)
                if (GameManager.instance.isGameOver) return;

            if (state == PauseState.Play)
            {
                HUDInterface.instance.ClearLoot();
                MenuManager.instance.OpenMenu(pauseMenu);
                state = PauseState.Pause;
            }
            else
            {
                CloseAllMenus();
                state = PauseState.Play;
            }

            Time.timeScale = (float)(state);

        }
    }

    public void Resume()
    {
        state = PauseState.Play;
        Time.timeScale = 1.0f;
        MenuManager.instance.CloseMenu(pauseMenu);
    }

    private void CloseAllMenus()
    {
        BaseMenu[] menus = FindObjectsOfType<BaseMenu>();
        foreach (BaseMenu menu in menus)
            menu.close();
    }
}

