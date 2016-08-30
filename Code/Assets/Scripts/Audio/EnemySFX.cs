using UnityEngine;
using System.Collections;

public class EnemySFX : MonoBehaviour 
{
    [SerializeField]
    private AudioClip walk;
    [SerializeField]
    private AudioClip projectile;
    [SerializeField]
    private AudioClip die;
    [SerializeField]
    private AudioClip damage;
    [SerializeField]
    private AudioClip melee;
    [SerializeField]
    private AudioClip hit;

    public void PlayWalkSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(walk, transform.position);
    }

    public void PlayProjectileSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(projectile, transform.position);
    }

    public void PlayDieSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(die, transform.position);
    }

    public void PlayDamageSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(damage, transform.position);
    }

    public void PlayMeleeSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(melee, transform.position);
    }

    public void PlayHitSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(hit, transform.position);
    }
}
