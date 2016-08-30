using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour 
{
    public int pierce;
    public float speed;
    public List<BaseEffectObject> effects = new List<BaseEffectObject>();

    private HitInfo damage;
    public HitInfo Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage.attackInfo = value.attackInfo;
            damage.element = value.element;
        }
    }
    private Entity owner;
    public Entity Owner
    {
        get
        {
            return owner;
        }

        set
        {
            owner = value;
        }
    }
    private Rigidbody body;

    public AudioClip hitSound;

    public bool playSound;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
        damage.effects = effects;

        Destroy(this.gameObject, 6.0f);
    }

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        body.MovePosition(body.position + transform.forward * step);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Floor")
        {
            PlaySound();
            Destroy(this.gameObject);
        }

        Entity target = col.GetComponent<Entity>();

        if (target == null || target == owner || target.entityType == owner.entityType) return;

        target.TakeDamage(ref damage);

        --pierce;

        DecreaseDamage();

        PlaySound();

        if (pierce <= 0)
            Destroy(this.gameObject);
    }

    void PlaySound()
    {
        if (playSound)
        {
            if (SoundManager.instance)
                SoundManager.instance.PlayClip(hitSound, transform.position);
        }
    }

    void DecreaseDamage()
    {
        damage.attackInfo.elemental = damage.attackInfo.elemental / 2.0f;
        damage.attackInfo.physical = damage.attackInfo.physical / 2.0f;
    }


}
