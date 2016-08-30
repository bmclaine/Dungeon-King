using UnityEngine;
using System.Collections;

public class GoE : Enemy
{
    [SerializeField]
    private Transform projectileLocation;
    [SerializeField]
    private Transform secondProjectileLocation;
    private Vector3 teleportDestination;

    private int attackstack;

	private void Start () 
    {
        init();
	}

    protected override void init()
    {
        enemyType = EnemyType.Goe;
        base.init();

        ChangeElement();
    }
	
	private void Update () 
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();
	}

    private void UpdateState()
    {
        switch(state)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Die:
                DieState();
                break;
        }
    }

    private void IdleState()
    {
        if (!attackTarget) return;

        UpdateDecisionCycle();

        if (decisionCycle > 0.0f) return;

        if(inRange() == true)
        {
            ChangeState(EnemyState.Ability);
            ResetDecision();
            return;
        }

        LookAtTarget();

        ChangeState(EnemyState.Projectile);

        ResetDecision();
    }

    private void Teleport()
    {
        SetTeleportDestination();

        if(ObjectManager.instance)
            Instantiate(ObjectManager.instance.GoeTeleportObject, soulDropPosition.position, transform.rotation);

        transform.position = teleportDestination;
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Goe, element);
        renderer.material.SetTexture(0, texture);
    }

    private void CreateDieEffect()
    {
        if(ObjectManager.instance)
            Instantiate(ObjectManager.instance.GoeDieEffectObject, soulDropPosition.position, transform.rotation);
    }

    private void CreateProjectile()
    {
        GameObject projectileTemplate = ObjectManager.instance.GetProjectile(element);
        GameObject projectileObj = (GameObject)Instantiate(projectileTemplate, projectileLocation.position, projectileLocation.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (!projectile) return;

        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void CreateSecondProjectile()
    {
        GameObject projectileTemplate = ObjectManager.instance.GetProjectile(element);
        GameObject projectileObj = (GameObject)Instantiate(projectileTemplate, secondProjectileLocation.position, projectileLocation.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (!projectile) return;

        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void SetTeleportDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius * 0.5f;
        randomDirection += anchorPosition.position;
        NavMeshHit hit;
        bool foundPos = false;
        while (!foundPos)
        {
            foundPos = NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            teleportDestination = hit.position;
        }
    }
}
