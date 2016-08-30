using UnityEngine;
using System.Collections;

public class DragonSFX : EnemySFX
{
    [SerializeField]
    private AudioClip beam;

    public void PlayBeamSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(beam, transform.position);
    }
}
