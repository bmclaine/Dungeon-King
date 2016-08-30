using UnityEngine;
using System.Collections;

public class Dragon : Enemy
{
    [SerializeField]
    private GameObject beamObj;
    [SerializeField]
    private Transform projectileLocation;

    [SerializeField]
    private GameObject[] breathObjects;

	private void Start () 
    {
        init();
	}

    protected override void init()
    {
        enemyType = EnemyType.Dragon;
        base.init();
        CancelAbility();
        SetBeam();
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

        if (inRange() == false)
        {
            agent.enabled = true;
            agent.SetDestination(attackTarget.transform.position);
            LookAtTarget();
        }
        else
        {
            LookAtTarget();
            agent.enabled = false;
            ChangeState(EnemyState.Attack);
        }
    }

    private void AttackDecision()
    {
        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision < 0.5f)
        {
            countDown = 5.0f;
            ChangeState(EnemyState.Pursue);
        }
        else
        {
            LookAtTarget();
            float action = Random.Range(0.0f, 1.0f);

            if (action < 0.75f)
                ChangeState(EnemyState.Projectile);
            else
                ChangeState(EnemyState.Ability);
        }
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.Dragon, element);
        renderer.material.SetTexture(0, texture);
    }

    private void UseAbility()
    {
        beamObj.SetActive(true);

        int index = (int)element;
        breathObjects[index].SetActive(true);
        breathObjects[index].GetComponentInChildren<ParticleSystem>().Play();
    }

    private void CancelAbility()
    {
        beamObj.SetActive(false);

        int index = (int)element;
        breathObjects[index].SetActive(false);
    }

    private void CreateProjectile()
    {
        GameObject projectileTemplate = ObjectManager.instance.GetProjectile(element);
        GameObject projectileObj = (GameObject)Instantiate(projectileTemplate, projectileLocation.position, projectileLocation.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void SetBeam()
    {
        Beam beam = beamObj.GetComponent<Beam>();
        HitInfo info = new HitInfo(attack);
        info.attackInfo.physical *= 2.0f;
        info.attackInfo.elemental *= 2.0f;
        beam.Damage = info;
        beam.Owner = this;
    }
}
