using UnityEngine;
using System.Collections;

public class HellHound : Enemy
{
    [SerializeField]
    private Transform projectileLocation;
    [SerializeField]
    private float breathRange;
    [SerializeField]
    private GameObject[] breathArray;

    private bool useAbility = false;

    // Use this for initialization
    void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.Hellhound;
        base.init();
        ChangeElement();
        BreathEnd();

        int index = (int)element;
        breathArray[index].GetComponent<AOE>().Damage = new HitInfo(attack);
        breathArray[index].GetComponent<AOE>().Owner = this;
    }

    // Update is called once per frame
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

        if (decisionCycle > 0.0f) return;

        float decision = Random.Range(0.0f, 1.0f);

        if (decision < aggresivness)
            AttackDecision();

        ResetDecision();
    }

    private void PursueState()
    {
        if (attackTarget == null || agent == null) return;

        CountDown();
        ChooseNewAttackTarget();

        if (!useAbility)
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
                LookAtTarget();
                ChangeState(EnemyState.Projectile);
            }
        }
        else
        {
            if (inRange(breathRange) == false)
            {
                agent.enabled = true;
                agent.SetDestination(attackTarget.transform.position);
                LookAtTarget();
            }
            else
            {
                agent.enabled = false;
                ChangeState(EnemyState.Ability);
            }
        }
    }

    #endregion

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Hellhound, element);
        renderer.material.SetTexture(0, texture);
    }

    private void Breath()
    {
        int index = (int)element;

        breathArray[index].SetActive(true);
        breathArray[index].GetComponentInChildren<ParticleSystem>().Play();
    }

    private void BreathEnd()
    {
        int index = (int)element;

        breathArray[index].SetActive(false);
    }

    private void AttackDecision()
    {
        useAbility = false;

        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision < 0.6f)
            ChangeState(EnemyState.Pursue);
        else
        {
            LookAtTarget();
            float action = Random.Range(0.0f, 1.0f);

            if (action < 0.5f)
            {
                LookAtTarget();
                agent.stoppingDistance = attackRange;
                ChangeState(EnemyState.Projectile);
            }
            else
            {
                LookAtTarget();
                useAbility = true;
                agent.stoppingDistance = breathRange;
                ChangeState(EnemyState.Pursue);
            }
        }

        countDown = 5.0f;
    }

    private bool inRange(float value)
    {
        if (attackTarget == null) return false;

        float distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        return distance <= value;
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
}
