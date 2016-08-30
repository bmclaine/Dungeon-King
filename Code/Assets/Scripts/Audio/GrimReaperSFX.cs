using UnityEngine;
using System.Collections;

public class GrimReaperSFX : MonoBehaviour
{
    [SerializeField]
    private AudioClip melee;

    [SerializeField]
    private AudioClip leave;

    public void PlayMeleeSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(melee, transform.position);
    }

    public void PlayLeaveSFX()
    {
        if (!SoundManager.instance) return;

        SoundManager.instance.PlayClip(leave, transform.position);
    }
}
