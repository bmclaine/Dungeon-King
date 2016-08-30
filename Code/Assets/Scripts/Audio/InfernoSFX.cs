using UnityEngine;
using System.Collections;

public class InfernoSFX : EnemySFX
{
    [SerializeField]
    private AudioClip explode;

    public void PlayExplodeSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(explode, transform.position);
    }
}
