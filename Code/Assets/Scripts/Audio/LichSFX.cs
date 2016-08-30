using UnityEngine;
using System.Collections;

public class LichSFX : EnemySFX
{
    [SerializeField]
    private AudioClip spawnMinions;

    [SerializeField]
    private AudioClip fade;

    public void PlaySpawnMinionsSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(spawnMinions, transform.position);
    }

    public void PlayFadeSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(fade, transform.position);
    }
}
