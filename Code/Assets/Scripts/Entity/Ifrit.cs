using UnityEngine;
using System.Collections;

public class Ifrit : Enemy
{
    private Vector3 wonderPosition;

    [SerializeField]
    private GameObject truthSeekerObj;
    [SerializeField]
    private GameObject divinePunishmentObj;

    [SerializeField]
    private Transform truthSeekerLocation;
    [SerializeField]
    private Transform divinePunishmentLocation;

    [SerializeField]
    private AudioClip damageSFX;
    [SerializeField]
    private AudioClip deathSFX;
    [SerializeField]
    private AudioClip truthSeekerVC;
    [SerializeField]
    private AudioClip divinePunshmentVC;
    [SerializeField]
    private AudioClip holyRayVC;

    private float yPos;

    // Use this for initialization
    void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.Ifrit;

        base.init();

        yPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();

        if (transform.position.y != yPos && state != EnemyState.Ability && state != EnemyState.Skill)
        {
            Vector3 temp = transform.position;
            temp.y = yPos;
            transform.position = temp;
        }
    }


    #region Enemey State Methods

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

            case EnemyState.Die:
                DieState();
                break;
        }
    }

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

    private void AttackDecision()
    {
        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision < 0.25f)
        {
            SetWonderPosition();
            ChangeState(EnemyState.Pursue);
        }
        else
        {
            LookAtTarget();
            float action = Random.Range(0.0f, 1.0f);

            if (action < 0.1f)
                ChangeState(EnemyState.Skill);
            else if (action > 0.1f && action < 0.7f)
                ChangeState(EnemyState.Projectile);
            else
                ChangeState(EnemyState.Ability);
        }
    }

    private void PursueState()
    {
        if (attackTarget == null || agent == null) return;

        agent.enabled = true;
        float distance = Vector3.Distance(transform.position, agent.destination);
        float diff = distance - agent.stoppingDistance;

        if (diff < 0.0f)
        {
            agent.enabled = false;
            ChangeState(EnemyState.Idle);
        }
    }

    #endregion

    private void DivinePunishment()
    {
        GameObject divinePunishment = (GameObject)Instantiate(divinePunishmentObj, divinePunishmentLocation.position, divinePunishmentLocation.rotation);
        Projectile projectile = divinePunishment.GetComponent<Projectile>();
        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void TruthSeeker()
    {
        GameObject truthSeeker = (GameObject)Instantiate(truthSeekerObj, truthSeekerLocation.position, truthSeekerLocation.rotation);
        Projectile projectile = truthSeeker.GetComponent<Projectile>();
        projectile.Owner = this;
        projectile.Damage = new HitInfo(attack);
    }

    private void HolyRay()
    {

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

    private void PlayDamageSFX()
    {
        SoundManager.instance.PlayClip(damageSFX, transform.position);
    }
    private void PlayDeathSFX()
    {
        SoundManager.instance.PlayClip(deathSFX, transform.position);
    }
    private void PlayTruthSeekerVC()
    {
        SoundManager.instance.PlayClip(truthSeekerVC, transform.position);
    }
    private void PlayDivinePunishmentVC()
    {
        SoundManager.instance.PlayClip(divinePunshmentVC, transform.position);
    }
    private void PlayHolyRayVC()
    {
        SoundManager.instance.PlayClip(holyRayVC, transform.position);
    }
}
