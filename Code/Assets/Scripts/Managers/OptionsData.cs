using UnityEngine;
using System.Collections;

public class OptionsData : object
{
    public float musicVolume;
    public float sfxVolume;
    public bool fullscreen;

    public OptionsData()
    {
        fullscreen = false;
    }

    public static void Save(OptionsData options)
    {
        PlayerPrefs.SetFloat("MusicVolume", options.musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", options.sfxVolume);
        int _fullScreen = System.Convert.ToInt32(options.fullscreen);
        PlayerPrefs.SetInt("Fullscreen", _fullScreen);
    }

    public static OptionsData Load()
    {
        OptionsData options = new OptionsData();

        options.musicVolume = PlayerPrefs.GetFloat("MusicVolume",100f);
        options.sfxVolume = PlayerPrefs.GetFloat("SFXVolume",100f);
        bool _fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 0));
        options.fullscreen = _fullScreen;

        return options;
    }
}
