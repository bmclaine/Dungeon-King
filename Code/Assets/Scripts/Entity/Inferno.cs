using UnityEngine;
using System.Collections;

public class Inferno : Enemy 
{
    [SerializeField]
    private Transform projectileLocation;

    private Vector3 wonderPosition;


    private void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.Inferno;
        base.init();
        ChangeElement();
    }

    private void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();
    }

    private void UpdateState()
    {
        if (health.current <= 0.0f && state != EnemyState.Die)
            ChangeState(EnemyState.Die);

        switch(state)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Move:
                MoveState();
                break;

            case EnemyState.Projectile:
                break;

            case EnemyState.Die:
                DieState();
                break;
        }
    }

    private void IdleState()
    {
        if (attackTarget == null) return;

        UpdateDecisionCycle();

        if (decisionCycle > 0.0f) return;

        float decision = Random.Range(0.0f, 1.0f);

        if(decision < aggresivness)
        {
            float action = Random.Range(0.0f, 1.0f);
            if (action < 0.75f)
            {
                Vector3 lookDirection = new Vector3(attackTarget.transform.position.x, transform.position.y, attackTarget.transform.position.z);
                transform.LookAt(lookDirection);
                ChangeState(EnemyState.Projectile);
            }
            else
            {
                SetWonderPosition();
                ChangeState(EnemyState.Move);
            }
        }

        ResetDecision();

    }

    private void MoveState()
    {
        if (health.current <= 0.0f)
            state = EnemyState.Die;

        float distance = Vector3.Distance(transform.position, agent.destination);
        if((distance - agent.stoppingDistance) < 0.1f)
        {
            agent.enabled = false;
            ChangeState(EnemyState.Idle);
        }
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Inferno, element);
        renderer.material.SetTexture(0, texture);
    }

    private void UseProjectile()
    {
        GameObject projectileTemplate = ObjectManager.instance.GetProjectile(element);
        GameObject projectileObj = (GameObject)Instantiate(projectileTemplate, projectileLocation.position, projectileLocation.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (!projectile) return;

        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void CreateExplosion()
    {
        GameObject explosionTemplate = ObjectManager.instance.GetExplosion(element);
        GameObject explosionObj = (GameObject)Instantiate(explosionTemplate, soulDropPosition.position, transform.rotation);
        AOE aoe = explosionObj.GetComponent<AOE>();
        aoe.Owner = this;
        aoe.Damage = new HitInfo(attack);
    }

    private void SetWonderPosition()
    {
        if (!agent) return;

        agent.enabled = true;
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += anchorPosition.position;
        NavMeshHit hit;
        bool foundPos = false;
        while (!foundPos)
        {
            foundPos = NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            wonderPosition = hit.position;
        }
        agent.SetDestination(wonderPosition);

    }
}
