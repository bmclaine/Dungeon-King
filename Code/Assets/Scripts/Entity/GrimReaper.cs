using UnityEngine;
using System.Collections;

public class GrimReaper : MonoBehaviour 
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private EnemyState state;
    [SerializeField]
    private GameObject leaveParticles;
    [SerializeField]
    private GameObject appearParticles;
    [SerializeField]
    private float giveUpTimer;

    private Animator anim;
    private NavMeshAgent agent;
    public Player player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateGiveUpTimer();
    }

    private void UpdateState()
    {
        if (!player) return;

        switch(state)
        {
            case EnemyState.Pursue:
                Pursue();
                break;
        }
    }

    private void Pursue()
    {
        if (giveUp())
            ChangeState(EnemyState.Die);

        if (inRange())
        {
            LookAtTarget();
            ChangeState(EnemyState.Attack);
        }
        else
            agent.SetDestination(player.transform.position); 
    }

    private void Appear()
    {
        if (appearParticles)
            Instantiate(appearParticles, transform.position, transform.rotation);
    }

    private void Leave()
    {
        if(leaveParticles)
            Instantiate(leaveParticles, transform.position, transform.rotation);

        Destroy(this.gameObject);
    }

    private void Attack()
    {
        InstantDeathEffectObject death = (InstantDeathEffectObject)ScriptableObject.CreateInstance("InstantDeathEffectObject");
        death.chance = 0.001f;
        if (player)
            player.AddEffect(death);
    }

    private void ChangeState(EnemyState _state)
    {
        state = _state;
        int index = (int)state;

        anim.SetInteger("ID", index);
    }

    private bool inRange()
    {
        if (!player) return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        return distance <= attackRange;
    }

    private bool giveUp()
    {
        return giveUpTimer <= 0.0f;
    }

    private void UpdateGiveUpTimer()
    {
        giveUpTimer -= 1 * Time.deltaTime;
    }

    private void LookAtTarget()
    {
        if (!player) return;

        Vector3 lookDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookDirection);
    }

}
