using UnityEngine;
using System.Collections;

public class CrushingWall : MonoBehaviour
{
    [SerializeField]
    private AudioClip dragging;
    [SerializeField]
    private AudioClip impact;

    private AudioSource audioSource;

    private Animator animator;

    public bool crush;


    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        crush = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (crush)
        {
            Player player = col.GetComponent<Player>();
            if (player == null) return;

            player.Die();
        }
    }

    void PlayImpact()
    {
        audioSource.clip = impact;
        audioSource.Play();
    }

    void PlayDraggin()
    {
        audioSource.clip = dragging;
        audioSource.Play();
    }

    void StopSound()
    {
        audioSource.Stop();
    }

    void FlipCrush()
    {
        crush = !crush;
    }
}
