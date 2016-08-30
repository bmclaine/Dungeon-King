using UnityEngine;
using System.Collections;

public class PenguinKnightAttackTarget : MonoBehaviour 
{
    private PenguinKnight owner;

    private void Start()
    {
        owner = transform.root.GetComponent<PenguinKnight>();
    }

    private void OnTriggerEnter(Collider col)
    {
        Entity entity = col.transform.root.GetComponent<Entity>();

        if (!entity) return;

        if (entity == owner) return;

        if (entity.Health.current <= 0.0f) return;

        if (entity.entityType != EntityType.Companion && entity.entityType != EntityType.Player)
            owner.AddAttackTarget(entity);
    }

}
