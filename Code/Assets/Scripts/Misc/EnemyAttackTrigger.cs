using UnityEngine;
using System.Collections;

public class EnemyAttackTrigger : MonoBehaviour 
{
    Entity owner;

    private void Start()
    {
        owner = transform.root.GetComponent<Entity>();
    }

    private void OnTriggerEnter(Collider col)
    {
        Entity entity = col.GetComponent<Entity>();

        if (!entity) return;

        if (entity.Health.current <= 0.0f) return;

        if (entity.entityType == EntityType.Enemy) return;

        if (entity.entityType == EntityType.Companion)
            owner.Attacktarget = entity;

        owner.AddAttackTarget(entity);
    }
}
