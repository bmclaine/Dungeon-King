using UnityEngine;
using System.Collections;

public class PlayerAttackTrigger : MonoBehaviour 
{
    public Player player;

    void OnTriggerEnter(Collider col)
    {
        Entity entity = col.transform.root.GetComponent<Entity>();
        if (entity == player) return;

        if (entity && entity.entityType != EntityType.Companion)
            player.AddAttackTarget(entity);
    }

    void OnTriggerExit(Collider col)
    {
        Entity entity = col.transform.root.GetComponent<Entity>();

        if (!entity) return;

        if (entity == player) return;

        if (entity.Health.current <= 0.0f) return;

        player.RemoveAttackTarget(entity);
    }
}
