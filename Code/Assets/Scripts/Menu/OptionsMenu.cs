using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : BaseMenu, ISavable, ILoadable
{
    public Selectable selectedOption;
    public Text musicVolumeText;
    public Slider musicVolumeSlider;
    public Text sfxVolumeText;
    public Slider sfxVolumeSlider;
    public Toggle fullScreenToggle;
    public float delay = 0.2f;

    [SerializeField]
    private bool isOptionsScene = false;

    void Start()
    {
        Load();

        fullScreenToggle.isOn = Screen.fullScreen;
    }

    void Update()
    {
        input();
    }

    public override void open()
    {
        view.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selectedOption.gameObject);
        selectedOption.Select();
        activeMenu = true;
    }

    public override void close()
    {
        view.SetActive(false);
        activeMenu = false;
        Save();
    }

    public override void input()
    {
        if(isOptionsScene)
        {
            if (Input.GetButtonDown("Cancel"))
                Exit();
        }   
    }

    public void ChangeMusicVolumeText()
    {
        musicVolumeText.text = musicVolumeSlider.value.ToString();
    }

    public void ChangeSFXVolumeText()
    {
        sfxVolumeText.text = sfxVolumeSlider.value.ToString();
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = fullScreenToggle.isOn;
        if (!fullScreenToggle.isOn)
        {
            Screen.SetResolution(PersistentInfo.screenWidth, PersistentInfo.screenHeight, false);
        }
    }

    public void Save()
    {
        OptionsData optionsData = new OptionsData();
        optionsData.musicVolume = Mathf.RoundToInt(musicVolumeSlider.value);
        optionsData.sfxVolume = Mathf.RoundToInt(sfxVolumeSlider.value);
        optionsData.fullscreen = fullScreenToggle.isOn;
        OptionsData.Save(optionsData);

        if(!isOptionsScene)
        {
            AudioSource[] sources = FindObjectsOfType<AudioSource>();

            foreach(AudioSource asource in sources)
            {
                asource.volume = optionsData.sfxVolume / 100.0f;
            }
        }
    }

    public void Load()
    {
        OptionsData optionsData = OptionsData.Load();
        musicVolumeSlider.value = optionsData.musicVolume;
        sfxVolumeSlider.value = optionsData.sfxVolume;
        fullScreenToggle.isOn = optionsData.fullscreen;

        Screen.fullScreen = fullScreenToggle.isOn;

        if(!fullScreenToggle.isOn)
        {
            Screen.SetResolution(PersistentInfo.screenWidth, PersistentInfo.screenHeight, Screen.fullScreen);
        }
    }

    public void Exit()
    {
        Save();
        PersistentInfo.LoadLevel("Main Menu");
    }

    public void TestSound()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.SetVolume(sfxVolumeSlider.value);
        SoundManager.instance.PlayButtonConfirm();
    }

    public void TestMusic()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.SetBGMusicVolume(musicVolumeSlider.value);
    }

    public void OpenPauseMenu()
    {
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();

        MenuManager.instance.CloseMenu(this);
        MenuManager.instance.OpenMenu(pauseMenu);
    }

}
