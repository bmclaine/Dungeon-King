using UnityEngine;
using System.Collections;

public class Salamander : Enemy
{
    private bool useAbility = false;
    [SerializeField]
    private float launchRange;
    [SerializeField]
    private Transform projectileLocation;
    [SerializeField]
    private Transform launchAbilityLocation;
    private float yPos;

    void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.Salamander;
        base.init();
        yPos = transform.position.y;

        ChangeElement();
    }

    void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();

        if (transform.position.y != yPos && state != EnemyState.Ability)
        {
            Vector3 temp = transform.position;
            temp.y = yPos;
            transform.position = temp;
        }
    }

    void UpdateState()
    {
        if (health.current <= 0.0f)
            ChangeState(EnemyState.Die);

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

    protected override void Flinch()
    {
        base.Flinch();
        ToggleCollider(false);
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
                ChangeState(EnemyState.Attack);
            }
        }
        else
        {
            if (inRange())
            {
                agent.enabled = false;
                ChangeState(EnemyState.Attack);
                return;
            }

            if (inRange(launchRange) == false)
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

    private void AttackDecision()
    {
        useAbility = false;

        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision < 0.5f)
            ChangeState(EnemyState.Pursue);
        else
        {
            LookAtTarget();
            float action = Random.Range(0.0f, 1.0f);

            if (action < 0.3f)
                ChangeState(EnemyState.Projectile);
            else
            {
                useAbility = true;
                ChangeState(EnemyState.Pursue);
            }
        }
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Salamander, element);
        renderer.material.SetTexture(0, texture);
    }

    private bool inRange(float value)
    {
        if (attackTarget == null) return false;

        float distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        return distance <= value;
    }

    private void LeapAbility()
    {
        GameObject leapTemplate = ObjectManager.instance.GetLeapAbility(element);
        GameObject leapObj = (GameObject)Instantiate(leapTemplate, launchAbilityLocation.position, transform.rotation);
        AOE leap = leapObj.GetComponent<AOE>();
        if (!leap) return;
        leap.Owner = this;
        leap.Damage = new HitInfo(attack);
    }

    private void ToggleCollider(bool value)
    {
        collider.isTrigger = value;
    }

    public void ChangeCollider()
    {
        collider.isTrigger = !collider.isTrigger;
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


