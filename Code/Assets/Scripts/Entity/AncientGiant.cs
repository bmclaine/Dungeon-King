using UnityEngine;
using System.Collections;

public class AncientGiant : Enemy
{
    [SerializeField]
    private SpawnData[] miniGiant;
    public bool isGiant;

    private int attackStack;

    // Use this for initialization
    void Start()
    {
        init();
    }

    protected override void init()
    {
        enemyType = EnemyType.AncientGiant;
        base.init();

        if(isGiant)
            ChangeElement();

        if (!isGiant)
            DivideStats();
    }

    private void DivideStats()
    {
        health.max = health.max / 2.0f;
        health.current = health.max;

        attack.physical = attack.physical / 2.0f;
        attack.elemental = attack.elemental / 2.0f;

        defense.physical = defense.physical / 2.0f;
        defense.elemental = defense.elemental / 2.0f;

        exp.max = exp.max / 2.0f;
        exp.current = exp.max;
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

    #region Enemey State Methods

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
        CountDown();
        ChooseNewAttackTarget();

        if (inRange() == false)
        {
            agent.enabled = true;
            if (attackTarget)
            {
                agent.SetDestination(attackTarget.transform.position);
                LookAtTarget();
            }
        }
        else
        {
            agent.enabled = false;
            ChangeState(EnemyState.Attack);
        }
    }

    private void AttackDecision()
    {
        float attackDecision = Random.Range(0.0f, 1.0f);

        if (attackDecision > 0.0f)
            ChangeState(EnemyState.Pursue);

        countDown = 5.0f;
    }

    private void NextAttack()
    {
        if (state == EnemyState.Die) return;

        float num = Random.Range(0.0f, 1.0f);

        bool attackDecision = num < aggresivness && attackTarget != null && inRange() && attackStack < 7;

        anim.SetBool("NextAttack", attackDecision);

        if (attackDecision == false)
        {
            ChangeState(EnemyState.Idle);
            attackStack = 0;
        }
        else
            attackStack++;
    }

    private void Split()
    {
        if (!isGiant) return;

        for (int i = 0; i < miniGiant.Length; i++)
        {
            GameObject miniAGiant = (GameObject)Instantiate(miniGiant[i].spawnObject, miniGiant[i].spawnPoint.position, transform.rotation);
            AncientGiant script = miniAGiant.GetComponent<AncientGiant>();
            script.isGiant = false;
            script.spawner = spawner;
            script.attackTarget = attackTarget;
            script.ChangeElement(element);
        }
        spawner.MaxEnemyCount++;
    }

    #endregion

    protected override void ChangeElement()
    {
        base.ChangeElement();

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.AncientGiant, element);
        renderer.material.SetTexture(0, texture);
    }

    public override void ChangeElement(Element _element)
    {
        base.ChangeElement(_element);

        Texture texture = ObjectManager.instance.GetEnemyTexture(EnemyType.AncientGiant, element);
        renderer.material.SetTexture(0, texture);
    }
}
