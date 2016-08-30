using UnityEngine;
using System.Collections;

public class BadAssLerpCrushingWall : MonoBehaviour
{
    [SerializeField]
    private Transform start;
    [SerializeField]
    private Transform end;
    [SerializeField]
    private float crushSpeed;
    [SerializeField]
    private float returnSpeed;
    [SerializeField]
    private float timer;

    private float startTime;
    private float journeyLength;

    private bool crush;
    private bool wait;

    [SerializeField]
    private AudioClip dragging;
    [SerializeField]
    private AudioClip slam;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        wait = false;
        crush = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(start.position, end.position);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wait == false)
        {
            if (crush)
                Crush();
            else
                Return();
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer < 0.0f)
            {
                Reset();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (crush && !wait)
        {
            Player player = col.GetComponent<Player>();
            if (player == null)
                return;
            //else
            //{
            player.TakeDamage(50);
                //player.knockback();
            //}
        }
    }

    void Crush()
    {
        float distCovered = (Time.time - startTime) * crushSpeed;

        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(start.position, end.position, fracJourney);

        if (transform.position == end.position)
        {
            audioSource.Stop();
            audioSource.clip = slam;
            audioSource.Play();
            timer = 1.0f;
            wait = true;
        }
    }

    void Return()
    {

        float distCovered = (Time.time - startTime) * returnSpeed;

        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(end.position, start.position, fracJourney);

        if (transform.position == start.position)
        {
            timer = 2.0f;
            wait = true;
        }
    }

    void Reset()
    {
        wait = false;
        startTime = Time.time;
        crush = !crush;

        if (!crush)
        {
            audioSource.clip = dragging;
            audioSource.Play();
        }
    }
}
