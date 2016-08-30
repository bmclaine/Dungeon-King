using UnityEngine;
using System.Collections;

public class FloorSpikes : MonoBehaviour
{
    [SerializeField]
    private AudioClip spikesUp;
    [SerializeField]
    private AudioClip spikesDown;

    private Animator animator;
    private float timer;
    private bool active;

    // Use this for initialization
    void Start()
    {
        active = false;
        timer = 0.0f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timer -= 1.0f * Time.deltaTime;

            if (timer <= 0.0f)
            {
                active = false;
                animator.SetBool("active", active);

                SoundManager.instance.PlayClip(spikesDown, transform.position);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();
        if (player == null) return;

        if (!active)
        {
            if (player == null)
                return;

            active = true;
            animator.SetBool("active", active);

            SoundManager.instance.PlayClip(spikesUp, transform.position);

            timer = 3.0f;
        }
        player.TakeDamage(25);
        player.KnockBack();
    }
}
