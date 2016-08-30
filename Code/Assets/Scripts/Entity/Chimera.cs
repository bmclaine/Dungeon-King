using UnityEngine;
using System.Collections;

public class Chimera : Enemy
{
    [SerializeField]
    private float aoeRange;
    [SerializeField]
    private float abilityRadius;

    [SerializeField]
    private GameObject aoeObject;
    [SerializeField]
    private Transform aoeTransform;
    [SerializeField]
    private GameObject shockWaveObject;
    [SerializeField]
    private Transform shockWaveTransform;
    [SerializeField]
    private GameObject underworldRisingObject;
    [SerializeField]
    private GameObject underworldRisingIndentifierObject;

    private Transform[] underworldRisingPositions;

    private EnemyState attackDecision;
    private int attackStack;

	private void Start () 
    {
        init();
	}

    protected override void init()
    {
        enemyType = EnemyType.Chimera;
        base.init();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (HUDInterface.instance)
            HUDInterface.instance.SetBossHealthBar(health.percent);
    }

    public override void TakeDamage(ref HitInfo damage)
    {
        base.TakeDamage(ref damage);

        if (HUDInterface.instance)
            HUDInterface.instance.SetBossHealthBar(health.percent);
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

            case EnemyState.Pursue:
                PursueState();
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

        float decision = Random.Range(0.0f, 1.0f);

        if(decision < aggresivness)
        {
            AttackDecision();
        }

        ResetDecision();
    }

    private void PursueState()
    {
        CountDown();
        ChooseNewAttackTarget();

        if (attackDecision == EnemyState.Skill)
        {
            if(inRange(aoeRange) == false)
            {
                agent.enabled = true;
                agent.SetDestination(attackTarget.transform.position);
                LookAtTarget();
            }
            else
            {
                agent.enabled = false;
                ChangeState(attackDecision);
            }
        }
        else
        {
            if (inRange() == false)
            {
                agent.enabled = true;
                agent.SetDestination(attackTarget.transform.position);
                LookAtTarget();
            }
            else
            {
                agent.enabled = false;
                ChangeState(attackDecision);
            }
        }  
    }

    private void AttackDecision()
    {
        float decision = Random.Range(0.0f, 1.0f);

        if (decision >= 0.8f)
        {
            LookAtTarget();
            ChangeState(EnemyState.Ability);
        }
        else if (decision < 0.8f && decision >= 0.6f)
        {
            LookAtTarget();
            ChangeState(EnemyState.Projectile);
        }
        else if (decision < 0.6f && decision >= 0.4f)
        {
            countDown = 5.0f;
            ChangeState(EnemyState.Pursue);
            attackDecision = EnemyState.Skill;
        }
        else
        {
            countDown = 5.0f;
            ChangeState(EnemyState.Pursue);
            attackDecision = EnemyState.Attack;
        }
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
            attackStack = 0;
        }
        else
            attackStack++;
    }

    private void CreateShockwave()
    {
        GameObject shockWave = (GameObject)Instantiate(shockWaveObject, shockWaveTransform.position, shockWaveTransform.rotation);
        Projectile projectile = shockWave.GetComponent<Projectile>();
        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void CreateAoE()
    {
        GameObject aoeGameObject = (GameObject)Instantiate(aoeObject, aoeTransform.position, aoeTransform.rotation);
        AOE aoe = aoeGameObject.GetComponent<AOE>();
        aoe.Owner = this;
        aoe.Damage = new HitInfo(attack);
    }

    private void CreateUnderworldRisingPositions()
    {
        underworldRisingPositions = new Transform[3];

        for(int i = 0; i < underworldRisingPositions.Length; ++i)
        {
            Vector3 position = Random.insideUnitSphere * (i + abilityRadius) + attackTarget.transform.position;
            Quaternion rotation = transform.rotation;

            GameObject trans = (GameObject)Instantiate(underworldRisingIndentifierObject, position, rotation);
            underworldRisingPositions[i] = trans.transform;
        }
    }

    private void CreateUnderworldRising()
    {
        HitInfo info = new HitInfo(attack);
        for(int i = 0; i < underworldRisingPositions.Length; ++i)
        {
            Vector3 position = underworldRisingPositions[i].position;
            position.y = 0;
            GameObject underworldObject = (GameObject)Instantiate(underworldRisingObject, position, transform.rotation);

            AOE aoe = underworldObject.GetComponent<AOE>();
            aoe.Owner = this;
            aoe.Damage = info;
        }
    }

    private bool inRange(float range)
    {
        if (attackTarget == null) return false;

        float distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        return distance <= range;
    }
}
