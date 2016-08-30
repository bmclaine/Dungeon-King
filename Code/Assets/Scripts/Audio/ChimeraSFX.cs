using UnityEngine;
using System.Collections;

public class ChimeraSFX : EnemySFX
{
    [SerializeField]
    private AudioClip aoeSFX;

    [SerializeField]
    private AudioClip abilitySFX;

    public void PlayAOESFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(aoeSFX,transform.position);
    }

    public void PlayAbilitySFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(abilitySFX, transform.position);
    }
}
