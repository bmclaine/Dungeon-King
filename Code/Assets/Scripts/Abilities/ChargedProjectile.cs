using UnityEngine;
using System.Collections;

public class ChargedProjectile : MonoBehaviour 
{
    private bool fire = false;
    private Rigidbody body;

    [SerializeField]
    private float chargeRate;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private AudioClip startSound;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private bool playSound;

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

    void Start()
    {
        body = GetComponent<Rigidbody>();

        Destroy(this.gameObject, 25.0f);
    }

	void FixedUpdate () 
    {
        if (!fire)
            Charge();
        else
            Move();
	}

    void Charge()
    {
        Vector3 scale = transform.localScale;
        scale.x += chargeRate * Time.deltaTime;
        scale.y += chargeRate * Time.deltaTime;
        scale.z += chargeRate * Time.deltaTime;
        transform.localScale = scale;
    }

    void Move()
    {
        body.MovePosition(body.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetFire(bool value)
    {
        fire = value;
    }

    void OnTriggerEnter(Collider col)
    {
        Entity target = col.GetComponent<Entity>();

        if (target == null || target == owner || target.entityType == owner.entityType) return;

        target.TakeDamage(ref damage);

        if (playSound)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }
}
