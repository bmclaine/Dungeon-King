using UnityEngine;
using System.Collections;

public class IfritSFX : EnemySFX 
{
    [SerializeField]
    private AudioClip holyRay;

    [SerializeField]
    private AudioClip truthSeeker;

    [SerializeField]
    private AudioClip divinePunishment;

    public void PlayHolyRaySFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(holyRay, transform.position);
    }

    public void PlayTruthSeeker()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(truthSeeker, transform.position);
    }

    public void PlayDivinePunishment()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(divinePunishment, transform.position);
    }
}
