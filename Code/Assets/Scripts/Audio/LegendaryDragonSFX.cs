using UnityEngine;
using System.Collections;

public class LegendaryDragonSFX : EnemySFX
{
    [SerializeField]
    private AudioClip shedSkin;

    [SerializeField]
    private AudioClip grandFireball;

    [SerializeField]
    private AudioClip flameTornado;

    public void PlayShedSkinSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(shedSkin, transform.position);
    }

    public void PlayGrandFireballSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(grandFireball, transform.position);
    }

    public void PlayFlameTornado()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(flameTornado, transform.position);
    }
}
