using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : Entity
{
    protected EnemyType enemyType;
    protected Animator anim;
    protected NavMeshAgent agent;
    new protected Collider collider;
    [SerializeField]
    protected Transform soulDropPosition;
    [SerializeField]
    protected Transform hitFXPosition;
    [SerializeField]
    new protected Renderer renderer;
    [SerializeField]
    protected Image healthBar;
    [SerializeField]
    protected Transform anchorPosition;
    [SerializeField]
    protected EnemyState state;
    public Element element;

    [Range(0.0f, 1.0f), SerializeField]
    protected float aggresivness;
    [SerializeField] protected float decisionCycle;
    [SerializeField] protected float attackRange;

    protected float maxDecisionCycle;
    
    [SerializeField]
    protected float walkRadius;
    [SerializeField]
    protected float outOfRangeDistance;

    public Transform AnchorPosition
    {
        get
        {
            return anchorPosition;
        }

        set
        {
            anchorPosition = value;
        }
    }
    public float WalkRadius
    {
        get
        {
            return walkRadius;
        }

        set
        {
            walkRadius = value;
        }
    }   

    protected EnemySpawner spawner;
    public EnemySpawner Spawner
    {
        get
        {
            return spawner;
        }

        set
        {
            spawner = value;
        }
    }

    public EnemyType Enemytype
    {
        get
        {
            return enemyType;
        }
    }

    protected float countDown;

    private void Start()
    {
        init();
    }

    protected virtual void init()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        state = EnemyState.Idle;
        entityType = EntityType.Enemy;
        SetStats(level);
        agent.stoppingDistance = attackRange;
        agent.speed = moveSpeed;
        maxDecisionCycle = decisionCycle;

        EntityManager.instance.AddEnemy(this);
    }

    private void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();
    }

    private void UpdateState()
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

    public virtual void ChangeState(EnemyState _state)
    {
        state = _state;
        int stateIndex = (int)state;
        anim.SetInteger("ID", stateIndex);
    }

    public void AttackTarget()
    {
        HitInfo info = new HitInfo(attack);
        if (attackTarget != null && inRange())
            attackTarget.TakeDamage(ref info);
    }

    #region Base Class Methods

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health.current <= 0.0f && state == EnemyState.Die) return;

        if (health.current <= 0.0f)
        {
            anim.SetTrigger("Die");
            ChangeState(EnemyState.Die);
        }

        if (healthBar)
            healthBar.fillAmount = health.percent;
    }

    public override void TakeDamage(ref HitInfo damage)
    {
        if (health.current <= 0.0f && state == EnemyState.Die) return;

        float critRate = Random.Range(0.0f, 1.0f);
        if (critRate < damage.attackInfo.critical)
        {
            damage.attackInfo.physical *= 1.25f;
            damage.attackInfo.elemental *= 1.25f;
        }

        base.TakeDamage(ref damage);

        float flinchRate = Random.Range(0.0f, 1.0f);

        if (health.current <= 0.0f)
        {
            anim.SetTrigger("Die");
            ChangeState(EnemyState.Die);
        }
        else
        {
            if (flinchRate < defense.flinch)
            {
                Flinch();
            }
        }

        CreateHitFX();

        if(healthBar)
            healthBar.fillAmount = health.percent;

        GetComponent<EnemySFX>().PlayHitSFX();
    }

    protected virtual void ChangeElement()
    {
        int index = Random.Range(1, 6);
        element = (Element)index;
    }

    public virtual void ChangeElement(Element _element)
    {
        element = _element;
    }

    protected void CreateHitFX()
    {
        if (!ObjectManager.instance) return;

        Vector3 position = hitFXPosition.position;
        Quaternion rotation = transform.rotation;
        Instantiate(ObjectManager.instance.enemyHitObject, position, rotation);
    }

    protected virtual void Flinch()
    {
        anim.SetTrigger("Damage");
        ChangeState(EnemyState.Damage);
        ResetDecision(maxDecisionCycle * 0.5f);
        agent.enabled = false;
    }

    public override void ModifyMoveSpeed(float value)
    {
        base.ModifyMoveSpeed(value);

        if(agent)
            agent.speed = moveSpeed * modifiers.speed;

        anim.speed = modifiers.speed;
    }

    public override void ResetSpeed()
    {
        EnemyInfo baseInfo = PersistentInfo.instance.getEnemy(enemyType);
        EnemyInfo statProgress = PersistentInfo.instance.getEnemyStatProgression(enemyType);

        moveSpeed = baseInfo.moveSpeed + (statProgress.moveSpeed * level);
        modifiers.speed = 1;
        agent.speed = moveSpeed;
        anim.speed = modifiers.speed;

        DeactivateFreeze();
    }

    protected void ChooseNewAttackTarget()
    {
        if (attackTarget == null) return;

        float distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        if (distance > outOfRangeDistance && countDown <= 0.0f)
        {
            int index = Random.Range(0, attackTargets.Count - 1);

            attackTarget = attackTargets[index];
        }
    }
   
    protected void CountDown()
    {
        countDown -= 1.0f * Time.deltaTime;
    }

    #endregion

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

    protected void DieState()
    {
        Color col = renderer.material.color;
        col.a -= 0.25f * Time.deltaTime;
        renderer.material.color = col;
        if (col.a <= 0.0f)
            Destroy(this.gameObject);
    }

    public virtual void Die()
    {
        DeactivateBurn();
        DeactivateFreeze();

        CreateSoul();

        if (spawner)
            spawner.EnemyCount++;

        EntityManager.instance.player.AddExp(exp.current);
        EntityManager.instance.RemoveEnemy(this);
        collider.enabled = false;
    }

    #endregion

    #region Helper Methods

    protected void UpdateDecisionCycle()
    {
        if (state == EnemyState.Die) return;

        if (decisionCycle > 0.0f)
            decisionCycle -= 1 * Time.deltaTime;
        else
            decisionCycle = 0.0f;
    }

    private void AttackDecision()
    {
        if (health.current <= 0.0f)
            state = EnemyState.Die;

        if (inRange())
            ChangeState(EnemyState.Attack);
        else
            ChangeState(EnemyState.Pursue);
    }

    protected void ResetDecision()
    {
        decisionCycle = Random.Range(0.1f, maxDecisionCycle);
    }

    private void ResetDecision(float max)
    {
        decisionCycle = Random.Range(0.1f, max);
    }

    protected bool inRange()
    {
        if (attackTarget == null) return false;

        float distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        return distance <= attackRange;
    }

    protected virtual void CheckNextAttack()
    {
        if (state == EnemyState.Die) return;

        float num = Random.Range(0.0f, 1.0f);

        bool attackDecision = num < aggresivness && attackTarget != null && inRange();

        anim.SetBool("NextAttack", attackDecision);

        if (attackDecision == false)
            ChangeState(EnemyState.Idle);
    }

    public override void KnockBack()
    {
        if (state == EnemyState.Die) return;

        ChangeState(EnemyState.Knockback);
        anim.SetTrigger("KnockBack");
    }

    protected void CreateSoul()
    {
        if (ObjectManager.instance)
            Instantiate(ObjectManager.instance.soulObject, soulDropPosition.position, Quaternion.identity);
    }

    protected void LookAtTarget()
    {
        if (!attackTarget) return;

        Vector3 lookDirection = new Vector3(attackTarget.transform.position.x, transform.position.y, attackTarget.transform.position.z);
        transform.LookAt(lookDirection);
    }

    public void SetStats(int level)
    {
        EnemyInfo baseInfo = PersistentInfo.instance.getEnemy(enemyType);
        EnemyInfo statProgress = PersistentInfo.instance.getEnemyStatProgression(enemyType);

        health.max = baseInfo.health.max + (statProgress.health.max * level);
        health.current = health.max;

        mana.max = baseInfo.mana.max + (statProgress.mana.max * level);
        mana.current = baseInfo.mana.current + (statProgress.mana.current * level);

        attack.physical = baseInfo.attack.physical + (statProgress.attack.physical * level);
        attack.elemental = baseInfo.attack.elemental + (statProgress.attack.elemental * level);
        attack.critical = baseInfo.attack.critical + (statProgress.attack.critical * level);

        defense.physical = baseInfo.defense.physical + (statProgress.defense.physical * level);
        defense.elemental = baseInfo.defense.elemental + (statProgress.defense.elemental * level);
        defense.flinch = baseInfo.defense.flinch - (statProgress.defense.flinch * level);
        exp.max = baseInfo.exp + (statProgress.exp * level);
        exp.current = exp.max;

        modifiers.speed = baseInfo.modifiers.speed + (statProgress.modifiers.speed * level);
        moveSpeed = baseInfo.moveSpeed;
    }

    #endregion
}
