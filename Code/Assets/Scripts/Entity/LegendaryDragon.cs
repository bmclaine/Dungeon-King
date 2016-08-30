using UnityEngine;
using System.Collections;

public class LegendaryDragon : Enemy
{
    [SerializeField]
    private GameObject grandFireballObj;
    [SerializeField]
    private GameObject flameTornadoObj;

    [SerializeField]
    private Transform fireballLocation;
    [SerializeField]
    private Transform[] tornadoLocation;

    private bool shedSkin = false;
    private Vector3 wonderPosition;

    private void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.LegendaryDragon;
        base.init();
    }

    private void Update()
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

            case EnemyState.Pursue:
                PursuitState();
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

        if (decisionCycle > 0.0f)
            return;

        if(health.current <= health.max * 0.3f && shedSkin == false)
        {
            ChangeState(EnemyState.Skill);
            shedSkin = true;
            return;
        }

        float decision = Random.Range(0.0f, 1.0f);

        if (decision < aggresivness)
            AttackDecision();

        ResetDecision();
    }

    private void PursuitState()
    {
        if (attackTarget == null || agent == null) return;

        CountDown();
        ChooseNewAttackTarget();

        agent.enabled = true;
        agent.SetDestination(attackTarget.transform.position);
        
        if(inRange())
        {
            agent.enabled = false;
            LookAtTarget();            
            ChangeState(EnemyState.Attack);
        }
    }

    private void ShedSkin()
    {
        health.current = health.max * 0.75f;
        defense.physical *= 0.75f;
        defense.elemental *= 0.75f;

        if(HUDInterface.instance)
            HUDInterface.instance.SetBossHealthBar(health.percent);
    }

    private void GrandFireball()
    {
        GameObject fireball = (GameObject)Instantiate(grandFireballObj, fireballLocation.position, fireballLocation.rotation);
        Projectile projectile = fireball.GetComponent<Projectile>();
        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void FlameTornado()
    {
        foreach(Transform location in tornadoLocation)
        {
            GameObject tornado = (GameObject)Instantiate(flameTornadoObj, location.position, location.rotation);
            Projectile projectile = tornado.GetComponent<Projectile>();
            projectile.Owner = this;
            projectile.Damage = new HitInfo(attack);
        }
    }

    public override void TakeDamage(ref HitInfo damage)
    {
        base.TakeDamage(ref damage);

        if(HUDInterface.instance)
            HUDInterface.instance.SetBossHealthBar(health.percent);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (HUDInterface.instance)
            HUDInterface.instance.SetBossHealthBar(health.percent);
    }

    public override void Die()
    {
        base.Die();
    }

    private void AttackDecision()
    {
        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision < 0.33f)
        {
            if (inRange() == false)
                ChangeState(EnemyState.Pursue);
            else
            {
                LookAtTarget();
                ChangeState(EnemyState.Attack);
            }
        }
        else
        {
            LookAtTarget();
            float action = Random.Range(0.0f, 1.0f);

            if (action < 0.55f)
                ChangeState(EnemyState.Projectile);
            else
                ChangeState(EnemyState.Ability);
        }

        countDown = 5.0f;
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
