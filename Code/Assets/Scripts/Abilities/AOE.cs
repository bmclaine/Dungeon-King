using UnityEngine;
using System.Collections.Generic;

public class AOE : MonoBehaviour
{
    private HitInfo damage;
    public HitInfo Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
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

    public List<BaseEffectObject> effects;
    public AudioClip sound;

    new private Collider collider;

    void Start()
    {
        if(SoundManager.instance)
            SoundManager.instance.PlayClip(sound, transform.position);
        collider = GetComponent<Collider>();
        //collider.enabled = false;
    }

    public void init()
    {
        collider = GetComponent<Collider>();
        collider.enabled = true;
    }

    void OnTriggerEnter(Collider col)
    {
        Entity target = col.GetComponent<Entity>();

        if (target == null || target == owner) return;

        if (target.entityType == EntityType.Companion && owner.entityType == EntityType.Player) return;

        if (target.entityType == owner.entityType) return;

        CreateHitInfo();

        target.TakeDamage(ref damage);
    }

    void CreateHitInfo()
    {
        damage.effects = new List<BaseEffectObject>();
        damage.effects.Clear();
        foreach (BaseEffectObject effect in effects)
        {
            damage.effects.Add(effect);
        }
    }
}
