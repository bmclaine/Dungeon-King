using UnityEngine;
using System.Collections;

public class SoulObject : MonoBehaviour 
{
    private Rigidbody body;
    private Transform player;

    [SerializeField]
    private float moveSpeed;

    private Renderer playerRenderer;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        Entity entity = EntityManager.instance.player;
        playerRenderer = entity.GetComponentInChildren<SkinnedMeshRenderer>();

        if (entity)
            player = entity.transform;
    }

    private void FixedUpdate()
    {
        if (!player) return;

        LookAtPlayer();

        float step = moveSpeed * Time.deltaTime;
        body.MovePosition(body.position + transform.forward * step);
    }

    private void LookAtPlayer()
    {
        Vector3 lookDirection = Vector3.zero;

        if(playerRenderer)
            lookDirection = playerRenderer.bounds.center;
        else
        {
            lookDirection = player.position;
            lookDirection.y = 1.0f;
        }

        transform.LookAt(lookDirection);
    }

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player)
        {
            player.SoulCount++;
            Destroy(this.gameObject);
        }
    }
}
