using UnityEngine;
using System.Collections;

public class HellhoundSFX : EnemySFX
{
    [SerializeField]
    private AudioClip breath;

    public void PlayBreathSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(breath, transform.position);
    }
}
