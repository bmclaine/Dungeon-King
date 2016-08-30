using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : BaseMenu
{
    public Selectable selectedOption;
    [SerializeField]
    private AudioClip mainMenuTheme;

    void Start()
    {
        if (selectedOption)
            selectedOption.Select();

        OptionsData data = OptionsData.Load();
        Screen.fullScreen = data.fullscreen;

        if (SoundManager.instance)
            SoundManager.instance.SetBgMusic(mainMenuTheme);
    }

    void Update()
    {
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
    }

    public void LoadLevel(string level)
    {
        PersistentInfo.LoadLevel(level);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
