using UnityEngine;
using System.Collections;

public class SalamanderSFX : EnemySFX
{
    [SerializeField]
    private AudioClip launch;

    public void PlayLaunchSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(launch, transform.position);
    }
}
