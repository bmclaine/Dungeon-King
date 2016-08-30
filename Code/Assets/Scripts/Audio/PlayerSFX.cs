using UnityEngine;
using System.Collections;

public class PlayerSFX : MonoBehaviour 
{
    [SerializeField]
    private AudioClip walkSFX;
    [SerializeField]
    private AudioClip meleeSFX;
    [SerializeField]
    private AudioClip dieSFX;
    [SerializeField]
    private AudioClip soulSiphonSFX;
    [SerializeField]
    private AudioClip summonSFX;
    [SerializeField]
    private AudioClip projectileSFX;
    [SerializeField]
    private AudioClip damageSFX;
    [SerializeField]
    private AudioClip iceAbilitySFX;
    [SerializeField]
    private AudioClip windAbilitySFX;
    [SerializeField]
    private AudioClip soulPulseSFX;

    public void PlayWalkSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(walkSFX, transform.position);
    }

    public void PlayMeleeSFX()
    {
        if(SoundManager.instance)
            SoundManager.instance.PlayClip(meleeSFX, transform.position);
    }

    public void PlayDieSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(dieSFX, transform.position);
    }

    public void PlaySoulSiphonSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(soulSiphonSFX, transform.position);
    }

    public void PlaySummonSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(summonSFX, transform.position);
    }

    public void PlayProjectileSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(projectileSFX, transform.position);
    }

    public void PlayDamageSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(damageSFX, transform.position);
    }

    public void PlayIceAbilitySFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(iceAbilitySFX, transform.position);
    }

    public void PlayWindAbilitySFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(windAbilitySFX, transform.position);
    }

    public void PlaySoulPulseSFX()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(soulPulseSFX, transform.position);
    }
}
