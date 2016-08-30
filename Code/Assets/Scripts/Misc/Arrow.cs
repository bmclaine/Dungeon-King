using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour 
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private AudioClip hitSound;

    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        Destroy(this.gameObject, 15f);
    }

    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        body.MovePosition(body.position + transform.forward * step);
    }

    private void OnTriggerEnter(Collider col)
    {
        Entity entity = col.GetComponent<Entity>();

        if(entity)
            entity.TakeDamage(damage);

        if (SoundManager.instance)
            SoundManager.instance.PlayClip(hitSound, transform.position);

        Destroy(this.gameObject);
    }
}
