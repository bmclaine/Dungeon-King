using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static SoundManager manager;
    public static SoundManager instance
    {
        get
        {
            if (manager) return manager;

            manager = FindObjectOfType<SoundManager>();
            return manager;
        }
    }

    public AudioClip buttonConfirm;
    public AudioClip buttonChange;
    public AudioSource backgroundMusic;
    private AudioSource audioSource;
    private OptionsData options;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (this != SoundManager.instance)
            Destroy(this.gameObject);

        audioSource = GetComponent<AudioSource>();

        options = OptionsData.Load();

        audioSource.volume = options.sfxVolume / 100f;
        backgroundMusic.volume = options.musicVolume / 100f;

        backgroundMusic.Play();
    }

    private void Update()
    {   
        if(backgroundMusic)
        {
            if (!backgroundMusic.isPlaying)
                backgroundMusic.Play();
        }
    }

    public void PlayButtonConfirm()
    {
        if (!audioSource)
            SetAudioSource();

        audioSource.clip = buttonConfirm;
        audioSource.Play();
    }

    public void PlayButtonChange()
    {
        if (!audioSource)
            SetAudioSource();

        audioSource.clip = buttonChange;
        audioSource.Play();
    }

    public void PlayClip(AudioClip clip, Vector3 point)
    {
        if (!clip) return;

        options = OptionsData.Load();
        float volume = options.sfxVolume;
        AudioSource.PlayClipAtPoint(clip, point, volume);
    }

    public void SetBgMusic(AudioClip clip)
    {
        if (!backgroundMusic) return;

        options = OptionsData.Load();
        backgroundMusic.gameObject.SetActive(true);
        backgroundMusic.volume = options.musicVolume / 100.0f;
        backgroundMusic.clip = clip;
        backgroundMusic.Play();
    }

    public void PlayBgMusic()
    {
        if(!backgroundMusic.isPlaying)
            backgroundMusic.Play();
    }

    public void SetBGMusicVolume(float volume)
    {
        if (!audioSource)
            SetAudioSource();

        audioSource.Stop();

        backgroundMusic.volume = volume / 100;

        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.Pause();

        if (!audioSource)
            SetAudioSource();

        audioSource.volume = volume / 100;
        audioSource.Play();
    }

    private void SetAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
