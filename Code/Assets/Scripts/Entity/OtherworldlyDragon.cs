using UnityEngine;
using System.Collections;

public class OtherworldlyDragon : Enemy
{
    [SerializeField]
    private Transform[] volleyPositions;
    [SerializeField]
    private Transform[] summonPositions;
    [SerializeField]
    private SpawnData[] summonObj;
    [SerializeField]
    private GameObject beamObj;
    [SerializeField]
    private Transform novaPosition;
    [SerializeField]
    private Collider[] colliders;

    private Vector3 wonderPosition;

    private GameObject novaObj;
    [SerializeField]
    private GameObject novaObjTemplate;

	private void Start () 
    {
        init();
	}

    protected override void init()
    {
        enemyType = EnemyType.OtherworldlyDragon;
        base.init();
        CanceBeam();
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

        if (decision < aggresivness)
            AttackDecision();

        ResetDecision();
    }

    private void PursueState()
    {
        if (attackTarget == null || agent == null) return;

        CountDown();
        ChooseNewAttackTarget();

        agent.enabled = true;
        float distance = Vector3.Distance(transform.position, agent.destination);
        float diff = distance - agent.stoppingDistance;

        if (diff < 0.1f)
        {
            agent.enabled = false;
            ChangeState(EnemyState.Idle);
        }
    }

    private void AttackDecision()
    {
        float changeElement = Random.Range(0.0f, 1.0f);

        if (changeElement < 0.3f)
            ChangeElement();

        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision < 0.25f)
        {
            SetWonderPosition();
            ChangeState(EnemyState.Pursue);
        }
        else
        {
            float action = Random.Range(0.0f, 1.0f);

            if (action > 0.8f)
            {
                ChangeState(EnemyState.Ability);
            }
            else if (action < 0.8f && action > 0.5f)
            {
                LookAtTarget();
                ChangeState(EnemyState.Projectile);
            }
            else if (action < 0.5f && action > 0.1f)
            {
                LookAtTarget();
                ChangeState(EnemyState.Skill);
            }
            else
            {
                ChangeState(EnemyState.Summon);
            }
                         
        }

        countDown = 5.0f;
    }

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.OtherworldlyDragon, element);
        renderer.material.SetTexture(0, texture);
    }

    private void Nova()
    {
        GameObject nova = (GameObject)Instantiate(novaObjTemplate, novaPosition.position, novaPosition.rotation);
        ChargedProjectile projectile = nova.GetComponent<ChargedProjectile>();
        nova.transform.SetParent(novaPosition);
        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
        projectile.SetFire(false);
        novaObj = nova;
    }

    private void FireNova()
    {
        if (!novaObj) return;

        ChargedProjectile projectile = novaObj.GetComponent<ChargedProjectile>();
        projectile.SetFire(true);
        novaObj.transform.SetParent(null);
        novaObj = null;
    }

    private void Volley()
    {
        for (int i = 0; i < volleyPositions.Length; ++i)
        {
            Vector3 volleyPos = volleyPositions[i].position + Random.insideUnitSphere * 5f;
            Quaternion rotation = volleyPositions[i].rotation;

            int projectileIndex = Random.Range(1, 6);
            GameObject projectileTemplate = ObjectManager.instance.GetProjectile((Element)projectileIndex);

            GameObject projectileObject = (GameObject)Instantiate(projectileTemplate, volleyPos, rotation);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Owner = this;
            projectile.Damage = new HitInfo(attack);
        }
    }

    private void Summon()
    {
        for(int i = 0; i < summonPositions.Length; ++i)
        {
            int enemyIndex = Random.Range(0, summonObj.Length - 1);
            SpawnData data = summonObj[enemyIndex];

            float rarity = Random.Range(0, 120);

            if (rarity > (int)data.rarity)
            {
                GameObject enemyObj = (GameObject)Instantiate(data.spawnObject,summonPositions[i].position,summonPositions[i].rotation);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                enemy.AnchorPosition = anchorPosition;
                enemy.Spawner = spawner;
                enemy.WalkRadius = walkRadius;
                enemy.Attacktarget = attackTarget;
            }
            else
                --i;
        }

        if (spawner)
            spawner.MaxEnemyCount += summonPositions.Length;
    }

    private void UseBeam()
    {
        beamObj.SetActive(true);
        Beam beam = beamObj.GetComponent<Beam>();
        beam.Owner = this;
        beam.Damage = new HitInfo(attack);
    }

    private void CanceBeam()
    {
        beamObj.SetActive(false);
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

    protected override void Flinch()
    {
        base.Flinch();
        CanceBeam();

        if(novaObj)
        {
            Destroy(novaObj);
            novaObj = null;
        }
    }

    public override void Die()
    {
        base.Die();
        for(int i = 0; i < colliders.Length; ++i)
        {
            Destroy(colliders[i]);
        }
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

    public override void AddEffect(BaseEffectObject effectObject)
    {
        if (!HUDInterface.instance) return;

        HUDInterface.instance.SetMessageWindow("Otherwordly dragon is immune to all effects");
    }
}
