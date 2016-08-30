using UnityEngine;
using System.Collections;

public class Yeti : Enemy 
{
    [SerializeField]
    private bool abilityUsed = false;
    private float attackStack;

    [SerializeField,Range(0.0f,1.0f)]
    private float shieldActivationTrigger;
    
    [SerializeField]
    private GameObject shieldEffectObject;

	private void Start () 
    {
        init();
	}

    protected override void init()
    {
        enemyType = EnemyType.Yeti;
        base.init();

        DisableShield();
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
        if (health.current <= health.max * shieldActivationTrigger && health.current > 0.0f && abilityUsed == false)
        {
            ChangeState(EnemyState.Ability);
            return;
        }

        if (health.current <= 0.0f && state != EnemyState.Die)
            ChangeState(EnemyState.Die);

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

    public override void TakeDamage(ref HitInfo damage)
    {
        if (state == EnemyState.Die && health.current <= 0.0f) return;

        damage.attackInfo.elemental *= 2.0f;
        if (abilityUsed)
        {
            damage.attackInfo.physical = 0.0f;
        }

        base.TakeDamage(ref damage);
    }

    private void IdleState()
    {
        if (!attackTarget) return;

        UpdateDecisionCycle();

        if (decisionCycle > 0.0f) return;

        float decision = Random.Range(0.0f, 1.0f);

        if (health.current <= health.max * shieldActivationTrigger && health.current > 0.0f && abilityUsed == false)
        {
            ChangeState(EnemyState.Ability);
            return;
        }
        else
        {
            if (decision < aggresivness)
            {
                AttackDecision();
                return;
            }
        }

        ResetDecision();
    }

    private void PursuitState()
    {
        if (attackTarget == null || agent == null) return;

        CountDown();
        ChooseNewAttackTarget();

        if (inRange() == false)
        {
            agent.enabled = true;
            agent.SetDestination(attackTarget.transform.position);
            LookAtTarget();
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

        LookAtTarget();
    }

    private void AttackDecision()
    {
        if (health.current <= 0.0f)
            state = EnemyState.Die;

        if (inRange())
        {
            LookAtTarget();
            ChangeState(EnemyState.Attack);
        }
        else
            ChangeState(EnemyState.Pursue);

        countDown = 5.0f;
    }

    private void NextAttack()
    {
        if (state == EnemyState.Die) return;

        float num = Random.Range(0.0f, 1.0f);

        bool attackDecision = num < aggresivness && attackTarget != null && inRange() && attackStack < 7 && health.current > 0.0f;

        anim.SetBool("NextAttack", attackDecision);

        if (attackDecision == false)
        {
            ChangeState(EnemyState.Idle);
            attackStack = 0.0f;
        }
        else
            attackStack++;
    }

    private void UseAbility()
    {
        abilityUsed = true;
        aggresivness = 0.9f;
        attack.physical *= 1.5f;
        attack.critical = 0.5f;
        defense.physical *= 1.5f;
        defense.elemental *= 0.5f;
        defense.flinch = 0.0f;

        shieldEffectObject.SetActive(true);
    }

    private void DisableShield()
    {
        shieldEffectObject.SetActive(false);
    }

    private void ActivateShield()
    {
        shieldEffectObject.SetActive(true);
    }

    public override void Die()
    {
        base.Die();

        DisableShield();
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Yeti, element);
        renderer.material.SetTexture(0, texture);
    }
}
