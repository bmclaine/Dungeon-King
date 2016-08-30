using UnityEngine;
using System.Collections;

public class GoeSFX : EnemySFX
{
    [SerializeField]
    private AudioClip teleport;

    public void PlayTeleportSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(teleport, transform.position);
    }
}
