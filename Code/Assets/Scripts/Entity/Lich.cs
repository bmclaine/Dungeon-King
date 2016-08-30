using UnityEngine;
using System.Collections;

public class Lich : Enemy
{
    [SerializeField]
    private GameObject summonObj;
    [SerializeField]
    private float summonRate;
    [SerializeField, Range(0.0f,1.0f)]
    private float alphaValue;

    [SerializeField]
    private Transform[] summonLocations;

    private float maxSummonRate;
    private Vector3 wonderPosition;
    private bool summonedMinions = false;

    private void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.Lich;
        base.init();
        maxSummonRate = summonRate;
        summonRate = 0.0f;
        ChangeElement();
    }

    private void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateSummonRate();

        UpdateEffects();
    }

    private void UpdateState()
    {
        switch(state)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Move:
                MoveState();
                break;

            case EnemyState.Die:
                DieState();
                break;

            case EnemyState.Attack:
                AttackState();
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
            if(inRange() && isFaded() == false)
            {
                LookAtTarget();
                ChangeState(EnemyState.Attack);
                ResetDecision();
                return;
            }

            if(summonRate <= 0.0f)
            {
                if (summonedMinions == false)
                {
                    //Change state to summon enemy state
                    if (isFaded() == false)
                    {
                        LookAtTarget();
                        ChangeState(EnemyState.Summon);
                    }
                    else
                    {
                        UnFade();
                    }
                }
                else
                {
                    if (isFaded() == false)
                        ChangeState(EnemyState.Ability);
                }
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
        float distance = Vector3.Distance(transform.position, agent.destination);
        if ((distance - agent.stoppingDistance) < 0.1f)
        {
            agent.enabled = false;
            ChangeState(EnemyState.Idle);
        }
    }

    private void AttackState()
    {
        if (health.current <= 0.0f)
            state = EnemyState.Die;
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Lich, element);
        renderer.material.SetTexture(0, texture);
    }

    private void Summon(int location)
    {
        if (health.current <= 0.0f)
        {
            state = EnemyState.Die;
            return;
        }

        summonedMinions = true;
        summonRate = maxSummonRate;
        GameObject summon = (GameObject)Instantiate(summonObj, summonLocations[location].position, summonLocations[location].rotation);
        Enemy enemy = summon.GetComponent<Enemy>();
        enemy.Attacktarget = attackTarget;
        enemy.Spawner = spawner;
        if (spawner)
            spawner.MaxEnemyCount++;
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

    private void Fade()
    {
        Color col = renderer.material.color;
        col.a = alphaValue;
        renderer.material.color = col;

        if (healthBar)
            healthBar.gameObject.SetActive(false);
    }

    private void UnFade()
    {
        Color col = renderer.material.color;
        col.a = 1.0f;
        renderer.material.color = col;

        if(healthBar)
            healthBar.gameObject.SetActive(true);
        
    }

    private bool isFaded()
    {
        Color col = renderer.material.color;

        return col.a < 0.9f;
    }

    private void UpdateSummonRate()
    {
        summonRate -= Time.deltaTime;

        if (summonRate < 0.0f)
        {
            summonedMinions = false;
            summonRate = 0.0f;
        }
    }
}
