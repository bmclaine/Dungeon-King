using UnityEngine;
using System.Collections.Generic;

public class PenguinKnight : Entity 
{
    [SerializeField]
    new private Renderer renderer;
    [SerializeField]
    private float attackRange;
    [SerializeField, Range(0.0f, 1.0f)]
    private float aggressiveness;

    [SerializeField]
    private float decisionCycle;
    private float maxCycle;

    [SerializeField]
    private CompanionState state;
    private Animator anim;
    private NavMeshAgent agent;

    [SerializeField]
    private Transform anchorPosition;
    private int attackStack;

    [SerializeField]
    private PlayerInfo stats;
    [SerializeField]
    private PlayerInfo statProgress;

	private void Start () 
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        maxCycle = decisionCycle;
        entityType = EntityType.Companion;
        EntityManager.instance.AddPenguinKnight(this);
	}
	
	private void Update () 
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();
	}

    private void UpdateState()
    {
        if (health.current <= 0.0f && state != CompanionState.Die)
            ChangeState(CompanionState.Die);

        switch(state)
        {
            case CompanionState.Idle:
                IdleState();
                break;
            case CompanionState.Pursuit:
                PursueState();
                break;

            case CompanionState.Die:
                DieState();
                break;
        }
    }

    private void IdleState()
    {
        if (attackTarget == null)
            ChangeTarget();

        if(attackTarget == null)
        {
            if (inRange(0) == false)
                ChangeState(CompanionState.Pursuit);
            else
                agent.enabled = false;

            return;
        }

        UpdateDecisionCycle();

        if (decisionCycle > 0.0f) return;

        if (attackTarget)
        {
            if (inRange())
                ChangeState(CompanionState.Attack);
            else
                ChangeState(CompanionState.Pursuit);
        }
        else
        {
            if (inRange(0) == false)
                ChangeState(CompanionState.Pursuit);
            else
                agent.enabled = false;
        }

        ResetDecisionCycle();
    }

    private void PursueState()
    {
        if(attackTarget)
        {
            if(inRange())
            {
                attackStack = 0;
                ChangeState(CompanionState.Attack);
                agent.enabled = false;
            }
            else
            {
                agent.enabled = true;
                agent.SetDestination(attackTarget.GetDamagePosition().position);
            }
        }
        else
        {
            if(inRange(0))
            {
                ChangeState(CompanionState.Idle);
                agent.enabled = false;
            }
            else
            {
                LookAtAnchor();
                agent.enabled = true;
                agent.SetDestination(anchorPosition.position);
            }
        }
    }

    private void DieState()
    {
        Color col = renderer.material.color;
        col.a -= 0.25f * Time.deltaTime;
        renderer.material.color = col;
        if (col.a <= 0.0f)
            Destroy(this.gameObject);
    }

    private bool inRange()
    {
        if (!attackTarget) return false;

        float distance = Vector3.Distance(transform.position, attackTarget.GetDamagePosition().position);

        return distance <= attackRange;
    }

    private bool inRange(float dist)
    {
        float distance = Vector3.Distance(transform.position, anchorPosition.position);

        NavMeshHit hit;
        bool foundPos = false;

        foundPos = NavMesh.SamplePosition(anchorPosition.position, out hit, 1, 1);

        if (foundPos == false)
            return true;

        return (distance - agent.stoppingDistance) < 0.1f;
    }

    protected void Flinch()
    {
        anim.SetTrigger("Damage");
        ChangeState(CompanionState.Damage);
        ResetDecisionCycle();
        agent.enabled = false;
    }

    public override void RemoveAttackTarget(Entity target)
    {
        attackTargets.Remove(target);

        if (attackTarget == target)
        {
            attackTarget = null;
            ChangeTarget();
        }
    }

    public override void AddAttackTarget(Entity target)
    {
        if (attackTargets.Contains(target) || state == CompanionState.Die) return;

        attackTargets.Add(target);

        if (!attackTarget)
            ChangeTarget();
        else
        {
            float attackTargetsDistance = Vector3.Distance(transform.position, attackTarget.GetDamagePosition().position);
            float newTargetDistance = Vector3.Distance(transform.position, target.GetDamagePosition().position);

            if (newTargetDistance < attackTargetsDistance)
                SetAttackTarget(target);
        }
    }

    public void ChangeTarget()
    {
        if (attackTargets.Count > 0)
        {
            int index = Random.Range(0, attackTargets.Count - 1);
            attackTarget = attackTargets[index];
            agent.enabled = true;
            agent.SetDestination(attackTarget.GetDamagePosition().position);
            ChangeState(CompanionState.Pursuit);
        }
        else
        {
            LookAtAnchor();
            agent.enabled = true;
            agent.SetDestination(anchorPosition.position);
            if (inRange(0) == false)
                ChangeState(CompanionState.Pursuit);
            else
                ChangeState(CompanionState.Idle);
        }
    }

    private void AttackTarget()
    {
        if (!attackTarget) return;

        HitInfo info = new HitInfo(attack);

        attackTarget.TakeDamage(ref info);
    }

    private void NextAttack()
    {
        LookAtTarget();
        if (state == CompanionState.Die) return;

        float num = Random.Range(0.0f, 1.0f);

        bool attackDecision = num < aggressiveness && attackTarget != null && inRange() && attackStack < 6;

        anim.SetBool("NextAttack", attackDecision);

        if (attackDecision == false)
        {
            ChangeState(CompanionState.Idle);
            attackStack = 0;
        }
        else
            attackStack++;
    }

    private void UpdateDecisionCycle()
    {
        decisionCycle -= Time.deltaTime;
    }

    private void ResetDecisionCycle()
    {
        decisionCycle = maxCycle;
    }

    private void LookAtTarget()
    {
        if (attackTarget == null) return;

        Vector3 lookDirection = attackTarget.GetDamagePosition().position;
        lookDirection.y = transform.position.y;

        transform.LookAt(lookDirection);
    }

    private void LookAtAnchor()
    {
        if (anchorPosition == null || state == CompanionState.Die) return;

        Vector3 lookDirecion = anchorPosition.position;
        lookDirecion.y = transform.position.y;

        transform.LookAt(lookDirecion);
    }

    public override void TakeDamage(float damage)
    {
        if (state == CompanionState.Die) return;

        base.TakeDamage(damage);

        if (health.current <= 0.0f)
        {
            anim.SetTrigger("Die");
            ChangeState(CompanionState.Die);
        }
    }

    public override void TakeDamage(ref HitInfo damage)
    {
        if (state == CompanionState.Die) return;

        base.TakeDamage(ref damage);

        float flinchRate = Random.Range(0.0f, 1.0f);

        if (health.current <= 0.0f)
        {
            anim.SetTrigger("Die");
            ChangeState(CompanionState.Die);
        }
        else
        {
            if (flinchRate < defense.flinch)
            {
                Flinch();
            }
        }
    }

    public void ChangeState(CompanionState _state)
    {
        state = _state;
        int id = (int)state;
        anim.SetInteger("ID", id);
    }

    public void SetAttackTarget(Entity target)
    {
        if (!attackTarget)
            attackTarget = target;

        if(target)
        {
            Transform damagePos = target.GetDamagePosition();
            if(damagePos)
            {
                agent = GetComponent<NavMeshAgent>();
                agent.enabled = true;
                agent.SetDestination(target.GetDamagePosition().position);
            }
        }
    }

    public void Die()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        agent.enabled = false;

        if(EntityManager.instance)
            EntityManager.instance.RemovePenguinKnight(this);
    }

    public void SetAnchorPosition(Transform pos)
    {
        anchorPosition = pos;
    }

    public void SetStats(int level)
    {
        health.max = stats.health.max + (statProgress.health.max * level);
        health.current = health.max;

        mana.max = stats.mana.max + (statProgress.mana.max * level);
        mana.current = stats.mana.current + (statProgress.mana.current * level);

        attack.physical = stats.attack.physical + (statProgress.attack.physical * level);
        attack.elemental = stats.attack.elemental + (statProgress.attack.elemental * level);
        attack.critical = stats.attack.critical + (statProgress.attack.critical * level);

        defense.physical = stats.defense.physical + (statProgress.defense.physical * level);
        defense.elemental = stats.defense.elemental + (statProgress.defense.elemental * level);
        defense.flinch = stats.defense.flinch - (statProgress.defense.flinch * level);

        modifiers.speed = stats.modifiers.speed + (statProgress.modifiers.speed * level);
        moveSpeed = stats.moveSpeed;
    }
}
