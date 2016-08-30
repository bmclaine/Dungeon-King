using UnityEngine;
using System.Collections;

public class Skeleton : Enemy
{
    void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.Skeleton;
        base.init();
    }

    void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();
    }

    void UpdateState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Pursue:
                PursueState();
                break;

            case EnemyState.Attack:
                AttackState();
                break;

            case EnemyState.Die:
                DieState();
                break;
        }
    }

    #region Enemy State Methods

    private void IdleState()
    {
        if (attackTarget == null) return;

        UpdateDecisionCycle();

        if (decisionCycle > 0.0f)
            return;

        float decision = Random.Range(0.0f, 1.0f);

        if (decision < aggresivness)
            AttackDecision();
        else
            ResetDecision();
    }


    private void PursueState()
    {
        if (attackTarget == null || agent == null) return;

        if (inRange() == false)
        {
            agent.enabled = true;
            agent.SetDestination(attackTarget.transform.position);
            Vector3 lookDirection = new Vector3(attackTarget.transform.position.x, transform.position.y, attackTarget.transform.position.z);
            transform.LookAt(lookDirection);
        }
        else
        {
            agent.enabled = false;
            ChangeState(EnemyState.Attack);
        }
    }

    private void AttackState()
    {
        if (health.current <= 0.0f)
            ChangeState(EnemyState.Die);

        if (attackTarget == null) return;

        Vector3 lookDirection = new Vector3(attackTarget.transform.position.x, transform.position.y, attackTarget.transform.position.z);
        transform.LookAt(lookDirection);

        if (!inRange())
            ChangeState(EnemyState.Pursue);
    }


    #endregion

    #region Helper Methods

    private void AttackDecision()
    {
        if (health.current <= 0.0f)
            state = EnemyState.Die;

        if (inRange())
            ChangeState(EnemyState.Attack);
        else
            ChangeState(EnemyState.Pursue);
    }

    public override void KnockBack()
    {
        if (state == EnemyState.Die) return;

        ChangeState(EnemyState.Knockback);
        anim.SetTrigger("KnockBack");
    }

    #endregion

}
