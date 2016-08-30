using UnityEngine;
using System.Collections;

public class StunTile : MonoBehaviour
{
    public bool active;
    public float resetTimer;
    public GameObject particle;
    public AudioClip audioClip;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        active = true;

        OptionsData options = OptionsData.Load();
        audioSource.volume = options.sfxVolume / 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            resetTimer -= 1.0f * Time.deltaTime;

            if (resetTimer <= 0.0f)
            {
                active = true;
                particle.SetActive(active);

                audioSource.Play();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();
            
        if (player == null) return;

        if (active)
        {
            active = false;
            particle.SetActive(active);

            audioSource.Stop();

            resetTimer = 5.0f;
            player.Stun();

            if(SoundManager.instance)
                SoundManager.instance.PlayClip(audioClip, transform.position);
        }
    }

}
