using UnityEngine;
using System.Collections;

public class MenuSFX : MonoBehaviour 
{
    public void PlayButtonChangeSFX()
    {
        if(SoundManager.instance)
            SoundManager.instance.PlayButtonChange();
    }

    public void PlayButtonConfirmSFX()
    {
        if(SoundManager.instance)
            SoundManager.instance.PlayButtonConfirm();
    }

    public void PlayBgMusic()
    {
        if(SoundManager.instance)
            SoundManager.instance.PlayBgMusic();
    }
}
