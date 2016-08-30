using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour 
{
    private HitInfo damage;
    private Entity owner;

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

    private void OnTriggerEnter(Collider col)
    {
        Entity entity = col.GetComponent<Entity>();

        if (entity == owner || entity == null) return;

        entity.TakeDamage(ref damage);
    }
}
