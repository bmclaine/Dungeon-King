using UnityEngine;
using System.Collections;

public class DragonSlayer : Player
{
    private void Awake()
    {
        init();
    }

    protected override void init()
    {
        base.init();
        playerType = PlayerType.DragonSlayer;
        ChangeElement(0);
        state = PlayerState.Idle;
        SetStats();
        SetStats(level);

        Load(PersistentInfo.saveData);
    }

    private void Update()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        UpdateState();

        UpdateEffects();

        UpdateLevelUpObjects();

        CheckChangeElement();

        AbilityCoolDown();

        SetMoveDirection();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case PlayerState.Idle:
                IdleState();
                break;

            case PlayerState.Move:
                MoveState();
                break;

            case PlayerState.Attack:
                CheckMove();
                Move();
                break;
        }
    }

    private void Move()
    {
        if (targetDirection == Vector3.zero)
            return;

        if (state != PlayerState.Attack)
        {
            Vector3 movement = moveDirection * moveSpeed * modifiers.speed * Time.deltaTime;
            controller.Move(movement);
        }

        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    #region State Methods

    private void IdleState()
    {
        ChangeState(PlayerState.Idle);

        if (CheckMove() == true)
        {
            ChangeState(PlayerState.Move);
            return;
        }

        if (CheckAttack() == true)
        {
            ChangeState(PlayerState.Attack);
            return;
        }

        if (CheckAbility() == true)
        {
            ChangeState(PlayerState.Ability);
            return;
        }

        if (CheckProjectile() == true)
        {
            ChangeState(PlayerState.Projectile);
            return;
        }
    }

    private void MoveState()
    {
        Move();

        if (CheckMove() == false)
        {
            ChangeState(PlayerState.Idle);
            return;
        }

        if (CheckAttack() == true)
        {
            ChangeState(PlayerState.Attack);
            return;
        }

        if(CheckProjectile() == true)
        {
            ChangeState(PlayerState.Projectile);
            return;
        }

        if (CheckAbility() == true)
        {
            ChangeState(PlayerState.Ability);
            return;
        }
    }

    private void AttackState()
    {
        bool attackSuccess = CheckAttack();
        anim.SetBool("NextAttack", attackSuccess);

        if (!attackSuccess)
        {
            if (CheckMove() == false)
                ChangeState(PlayerState.Idle);
            else
                ChangeState(PlayerState.Move);
        }
    }

    protected override void AttackTarget()
    {
        foreach (Entity target in attackTargets)
        {
            HitInfo hitInfo = CreateHitInfo();
            Enemy enemy = (target as Enemy);

            if(enemy)
            {
                switch(enemy.Enemytype)
                {
                    case EnemyType.Dragon:
                        hitInfo.attackInfo.physical *= 1.5f;
                        hitInfo.attackInfo.elemental *= 1.5f;
                        hitInfo.attackInfo.critical += 0.25f;
                        break;

                    case EnemyType.LegendaryDragon:
                        hitInfo.attackInfo.physical *= 1.5f;
                        hitInfo.attackInfo.elemental *= 1.5f;
                        hitInfo.attackInfo.critical += 0.25f;
                        break;

                    case EnemyType.OtherworldlyDragon:
                        hitInfo.attackInfo.physical *= 1.5f;
                        hitInfo.attackInfo.elemental *= 1.5f;
                        hitInfo.attackInfo.critical += 0.25f;
                        break;
                }
            }

            target.TakeDamage(ref hitInfo);
            if (divineLightActivated)
            {
                RestoreHealth(100);
            }
        }
    }

    #endregion

    #region Abilities

    public void BaseAbility()
    {
        int index = (int)element;
        SoulPulse soulPulse = abilities[index].abilityObject.GetComponent<SoulPulse>();
        soulPulse.ToggleActive(true);
        soulPulse.Activate();

        abilities[index].coolDown.MaxOut();
        UseMana(abilities[index].manaCost);
    }

    public void FireAbility()
    {
        HitInfo info = new HitInfo(attack);
        info.attackInfo = attack;
        info.attackInfo.physical = 0.0f;
        info.element = Element.Fire;
        info.effects.Clear();

        int index = (int)element;
        GameObject fireAbility = (GameObject)Instantiate(abilities[index].abilityObject, abilities[index].abilityLocation.position, Quaternion.identity);
        AOE aoe = fireAbility.GetComponent<AOE>();
        aoe.Owner = this;
        aoe.Damage = info;
        aoe.init();

        abilities[index].coolDown.MaxOut();
        UseMana(abilities[index].manaCost);
    }

    public void IceAbility()
    {
        HitInfo info = new HitInfo(attack);
        info.attackInfo = attack;
        info.element = Element.Water;

        int index = (int)element;
        Vector3 position = abilities[index].abilityLocation.position;
        Quaternion rotation = abilities[index].abilityLocation.rotation;
        GameObject iceBeam = (GameObject)Instantiate(abilities[index].abilityObject, position, rotation);
        Projectile projectile = iceBeam.GetComponent<Projectile>();
        projectile.Owner = this;
        projectile.Damage = info;

        abilities[index].coolDown.MaxOut();
        UseMana(abilities[index].manaCost);
    }

    public void WindAbility()
    {
        int index = (int)element;

        GameObject feetOfFury = (GameObject)Instantiate(abilities[index].abilityObject, Vector3.zero, Quaternion.identity);
        EffectObject effectObj = feetOfFury.GetComponent<EffectObject>();
        effectObj.Target = this;
        effectObj.ActivateEffect();

        abilities[index].coolDown.MaxOut();
        UseMana(abilities[index].manaCost);
    }

    public void LightAbility()
    {
        int index = (int)element;

        GameObject divineLight = (GameObject)Instantiate(abilities[index].abilityObject, Vector3.zero, Quaternion.identity);
        EffectObject effectObj = divineLight.GetComponent<EffectObject>();
        effectObj.Target = this;
        effectObj.ActivateEffect();

        abilities[index].coolDown.MaxOut();
        UseMana(abilities[index].manaCost);
    }

    public void DarkAbility()
    {
        EntityManager.instance.RemoveAllPenguinKnights();

        int index = (int)element;

        for (int i = 0; i < summonLocations.Length; ++i)
        {
            Vector3 position = summonLocations[i].position;
            Quaternion rotation = summonLocations[i].rotation;

            GameObject knightObj = (GameObject)Instantiate(abilities[index].abilityObject, position, rotation);
            PenguinKnight penguinKnight = knightObj.GetComponent<PenguinKnight>();
            penguinKnight.SetStats(level);
            penguinKnight.SetAnchorPosition(summonLocations[i]);
            
        }

        abilities[index].coolDown.MaxOut();
        UseMana(abilities[index].manaCost);
    }

    private void CreateProjectile()
    {
        GameObject projectileGO = ObjectManager.instance.GetProjectile(element);

        GameObject projectileObj = (GameObject)Instantiate(projectileGO, projectilePosition.position, projectilePosition.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Owner = this;

        HitInfo info = new HitInfo(attack);
        info.attackInfo.physical = 0.0f;
        projectile.Damage = info;
    }

    #endregion
}
