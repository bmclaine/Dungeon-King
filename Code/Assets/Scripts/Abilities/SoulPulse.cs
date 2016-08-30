using UnityEngine;
using System.Collections;

public class SoulPulse : MonoBehaviour
{
    public AudioClip sfx;
    public float timer;
    private float maxTimer;
    new private Collider collider;
    new private Renderer renderer;

    void Start()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        ToggleActive(false);
        maxTimer = timer;
    }

    void Update()
    {
        if (timer > 0.0f)
            timer -= 1.0f * Time.deltaTime;
        else
            ToggleActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        Enemy target = col.GetComponent<Enemy>();

        if (target == null) return;

        target.KnockBack();
    }

    public void ToggleActive(bool value)
    {
        collider.enabled = value;
        renderer.enabled = value;
    }

    public void Activate()
    {
        timer = maxTimer;

        if(SoundManager.instance)
            SoundManager.instance.PlayClip(sfx, transform.position);
    }
}
