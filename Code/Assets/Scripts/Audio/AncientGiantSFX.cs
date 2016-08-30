using UnityEngine;
using System.Collections;

public class AncientGiantSFX : EnemySFX
{
    [SerializeField]
    private AudioClip split;

    [SerializeField]
    private AudioClip punch;

    [SerializeField]
    private AudioClip smash;

    public void PlaySplitSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(split, transform.position);
    }

    public void PlayPunchSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(punch, transform.position);
    }

    public void PlaySmashSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(smash, transform.position);
    }
}
