using UnityEngine;
using System.Collections;

public class OtherwordlyDragonSFX : EnemySFX
{
    [SerializeField]
    private AudioClip nova;

    [SerializeField]
    private AudioClip volley;

    [SerializeField]
    private AudioClip beam;

    [SerializeField]
    private AudioClip summonMinions;

    public void PlayNovaSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(nova, transform.position);
    }

    public void PlayVolleySFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(volley, transform.position);
    }

    public void PlayBeamSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(beam, transform.position);
    }

    public void PlaySummonMinionsSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(summonMinions, transform.position);
    }
}
