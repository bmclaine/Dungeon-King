using UnityEngine;
using System.Collections;

public class YetiSFX : EnemySFX
{
    [SerializeField]
    private AudioClip shield;
    [SerializeField]
    private AudioClip melee2;
    [SerializeField]
    private AudioClip melee3;

    public void PlayShield()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(shield, transform.position);
    }

    public void PlayMelee2()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(melee2, transform.position);
    }

    public void PlayMelee3()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(melee3, transform.position);
    }
}
