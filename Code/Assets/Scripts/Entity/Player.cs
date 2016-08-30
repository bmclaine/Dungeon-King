using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : Entity
{
    protected PlayerType playerType;
    public PlayerState state;
    protected Animator anim;
    protected CharacterController controller;
    protected Element m_element;
    [SerializeField]
    protected int soulCount;
    [SerializeField]
    protected GameObject levelUpObject;
    [SerializeField]
    protected Text levelUpText;
    [SerializeField]
    protected Transform projectilePosition;
    protected RectTransform canvas;
    protected Transform mainCamera;
    [SerializeField]
    new protected Renderer renderer;

    protected bool divineLightActivated;

    [SerializeField]
    protected GameObject divineLightParticles;
    [SerializeField]
    protected Transform[] summonLocations;

    protected float y;
    protected float levelUpTextTimer;

    //--------------Movement--------------
    protected float h = 0.0f;
    protected float v = 0.0f;
    protected float rotationSpeed = 1000.0f;
    protected Vector3 targetDirection;
    protected Vector3 moveDirection;
    //------------------------------------

    public Element element
    {
        get
        {
            return m_element;
        }
    }
    public PlayerType Playertype
    {
        get
        {
            return playerType;
        }
    }
    public int SoulCount
    {
        get
        {
            return soulCount;
        }
        set
        {
            soulCount = value;
        }
    }

    public Ability[] abilities;
    private float ExpDifference;

    [SerializeField]
    private PlayerInfo baseStats;
    [SerializeField]
    private PlayerInfo statProgression;

    void Start()
    {
        init();
    }

    protected virtual void init()
    {
        entityType = EntityType.Player;
        playerType = PlayerType.None;
        ToggleDivineLight(false);
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        canvas = (RectTransform)GetComponentInChildren<Canvas>().transform;
        y = transform.position.y;
        EntityManager.instance.AddPlayer(this);
        mainCamera = Camera.main.transform;
        level = 1;

        if (HUDInterface.instance)
        {
            HUDInterface.instance.SetHealthBar(health.percent);
            HUDInterface.instance.SetManaBar(mana.percent);
            HUDInterface.instance.SetElementDisplay(element);
            HUDInterface.instance.SetExpBar(exp.percent);

            int index = (int)element;
            HUDInterface.instance.SetCooldown(abilities[index].coolDown.percent);
        }
    }

    public void ChangeState(PlayerState _state)
    {
        state = _state;
        int stateIndex = (int)state;

        anim.SetInteger("ID", stateIndex);
    }

    protected virtual void AttackTarget()
    {
        foreach (Entity target in attackTargets)
        {
            HitInfo hitInfo = CreateHitInfo();
            target.TakeDamage(ref hitInfo);
            if (divineLightActivated)
            {
                RestoreHealth(25);
            }
        }
    }

    public override void ModifyMoveSpeed(float value)
    {
        modifiers.speed += value;

        if (modifiers.speed < 1.0f)
            modifiers.speed = 1.0f;
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);

        if(HUDInterface.instance)
            HUDInterface.instance.SetHealthBar(health.percent);

        if (state == PlayerState.Die)
        {
            if (health.current > 0.0f)
                ChangeState(PlayerState.Idle);
        }
    }

    public override void RestoreMana(float value)
    {
        base.RestoreMana(value);

        if (HUDInterface.instance)
            HUDInterface.instance.SetManaBar(mana.percent);
    }

    public override void UseMana(float value)
    {
        base.UseMana(value);

        if (HUDInterface.instance)
            HUDInterface.instance.SetManaBar(mana.percent);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health.current <= 0.0f && state != PlayerState.Die)
        {
            anim.SetTrigger("Die");
            ChangeState(PlayerState.Die);
        }
        else
        {
            float flinchRate = Random.Range(0.0f, 1.0f);

            if (flinchRate < defense.flinch)
            {
                Flinch();
            }
        }

        if (HUDInterface.instance)
            HUDInterface.instance.SetHealthBar(health.percent);
    }

    public override void TakeDamage(ref HitInfo damage)
    {
        base.TakeDamage(ref damage);

        if (health.current <= 0.0f && state != PlayerState.Die)
        {
            anim.SetTrigger("Die");
            ChangeState(PlayerState.Die);
        }

        if (HUDInterface.instance)
            HUDInterface.instance.SetHealthBar(health.percent);
    }

    public override void AddExp(float value)
    {
        exp.current += value;

        LevelUp();

        if (!ObjectManager.instance) return;

        GameObject expObj = (GameObject)Instantiate(ObjectManager.instance.playerExpGainObject, canvas.position, canvas.rotation);
        RectTransform expTransform = (RectTransform)expObj.transform;
        expTransform.SetParent(canvas);
        expTransform.localScale = Vector3.one;
        expTransform.localPosition = Vector3.zero;
        Text expText = expObj.GetComponent<Text>();
        expText.text = "+" + Mathf.RoundToInt(value).ToString();
    }

    public override void LevelUp()
    {
        ExpDifference = exp.current - exp.max;

        if (exp.current > exp.max)
        {
            level++;
            exp.max *= 2;
            SetStats(level);
            levelUpObject.SetActive(true);
            levelUpText.gameObject.SetActive(true);
            levelUpText.text = "Level " + level;
            levelUpTextTimer = 3.0f;
        }

        float percent = (exp.current - ExpDifference) / exp.max;

        if (ExpDifference < 0.0f)
            percent = exp.percent;

        if(HUDInterface.instance)
            HUDInterface.instance.SetExpBar(percent);

        if (ExpDifference > 0.0f)
            exp.current = ExpDifference;

        if (HUDInterface.instance)
            HUDInterface.instance.SetHealthBar(health.percent);
    }

    public override void ResetHealth()
    {
        base.ResetHealth();

        if (HUDInterface.instance)
            HUDInterface.instance.SetHealthBar(health.percent);
    }

    protected HitInfo CreateHitInfo()
    {
        HitInfo info = new HitInfo(attack);
        info.attackInfo.elemental = 0.0f;

        if(InventoryManager.instance)
        {
            Weapon weapon = InventoryManager.instance.currentWeapon;
            if(weapon == null || weapon.itemType != ItemType.Weapon) return info;

            info.attackInfo.AddCritical(weapon.damage.critical);
            info.attackInfo.physical += weapon.damage.physical;
            info.attackInfo.elemental += weapon.damage.elemental;
            info.element = weapon.element;
            
            if(weapon.effect != null)
                info.effects.Add(weapon.effect);
        }

        return info;
    }

    public virtual void ChangeElement(Element _element)
    {
        m_element = _element;
        
        int elementId = (int)_element;
        anim.SetInteger("ElementID", elementId);

        if (HUDInterface.instance)
            HUDInterface.instance.SetElementDisplay(_element);

        Material mat = renderer.material;
        Texture texture = ObjectManager.instance.GetPlayerTexture(playerType, element);
        mat.SetTexture(0, texture);
    }

    public virtual void ChangeElement(int value)
    {
        Element affinity = m_element;
        affinity += value;
        
        if (affinity > Element.Dark)
            affinity = Element.None;

        if (affinity < Element.None)
            affinity = Element.Dark;

        ChangeElement(affinity);
    }

    public void SetAttackTarget(Entity entity)
    {
        if (attackTarget == null)
            attackTarget = entity;
    }

    protected void Flinch()
    {
        anim.SetTrigger("Damage");
        ChangeState(PlayerState.Damage);
    }

    public void Fall()
    {
        controller.enabled = false;
        ChangeState(PlayerState.Falling);
        anim.SetTrigger("Fall");
    }


    public void Die()
    {
        EntityManager.instance.RemovePlayer();

        if (GameManager.instance)
            GameManager.instance.PlayerDie(this);
    }

    public void ToggleDivineLight(bool value)
    {
        divineLightActivated = value;
        divineLightParticles.SetActive(value);
    }

    #region Helper Methods

    protected bool CheckMove()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float dpadX = Input.GetAxis("DPadX");
        float dpadY = Input.GetAxis("DPadY");

        if (dpadX != 0.0f || dpadY != 0.0f)
            return false;

        if (h != 0.0f || v != 0.0f)
            return true;

        return false;
    }

    protected bool CheckAttack()
    {
        return Input.GetButton("MeleeAttack");
    }

    protected bool CheckBlock()
    {
        return Input.GetKey(KeyCode.R);
    }

    protected bool CheckProjectile()
    {
        return Input.GetButtonDown("ProjectileAttack");
    }

    public override void KnockBack()
    {
        if (health.current <= 0.0f) return;

        anim.SetTrigger("KnockBack");
        ChangeState(PlayerState.Knockback);
    }

    public override void Stun()
    {
        anim.SetTrigger("Stun");
        ChangeState(PlayerState.Stun);
    }

    protected bool CheckAbility()
    {
        if (abilities.Length == 0)
            return false;

        int index = (int)element;

        if (index >= abilities.Length)
            return false;

        if (abilities[index].coolDown.current > 0.0f)
            return false;

        return (mana.current >= abilities[index].manaCost && abilities[index].unlocked == true && Input.GetButtonDown("Special Ability"));
    }

    protected void CheckChangeElement()
    {
        if (state == PlayerState.Die) return;

        if (Input.GetButtonDown("AffinityLeft"))
        {
            ChangeElement(-1);
        }

        if (Input.GetButtonDown("AffinityRight"))
        {
            ChangeElement(1);
        }
    }

    protected void AbilityCoolDown()
    {
        for (int i = 0; i < abilities.Length; ++i)
        {
            float regen = -1;
            abilities[i].coolDown.regen = regen;
            abilities[i].coolDown.Regenerate();
        }

        int index = (int)element;

        if (HUDInterface.instance)
            HUDInterface.instance.SetCooldown(abilities[index].coolDown.percent);
    }

    protected virtual void UpdateLevelUpObjects()
    {
        levelUpTextTimer -= Time.deltaTime;

        if(levelUpTextTimer < 0.0f)
        {
            levelUpText.gameObject.SetActive(false);
            levelUpObject.SetActive(false);
        }
    }

    public override void ResetSpeed()
    {
        modifiers.speed = 1.0f;
    }

    protected void SetMoveDirection()
    {
        Vector3 forward = mainCamera.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        targetDirection = h * right + v * forward;

        if (targetDirection != Vector3.zero)
        {
            //moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
            moveDirection = targetDirection;
            moveDirection = moveDirection.normalized;
        }
    }

    protected void SetStats()
    {
        health = baseStats.health;
        mana = baseStats.mana;
        attack = baseStats.attack;
        defense = baseStats.defense;
        modifiers = baseStats.modifiers;
        moveSpeed = baseStats.moveSpeed;

        exp.current = 0;
        exp.max = 100;

        if (!HUDInterface.instance) return;

        HUDInterface.instance.SetHealthBar(health.percent);
        HUDInterface.instance.SetManaBar(mana.percent);
        HUDInterface.instance.SetExpBar(exp.percent);
        HUDInterface.instance.SetElementDisplay(element);
    }

    protected void SetStats(int level)
    {
        health.max = baseStats.health.max + (statProgression.health.max * level);
        health.current = health.max;

        mana.max = baseStats.mana.max + (statProgression.mana.max * level);
        mana.current = baseStats.mana.current + (statProgression.mana.current * level);

        attack.physical = baseStats.attack.physical + (statProgression.attack.physical * level);
        attack.elemental = baseStats.attack.elemental + (statProgression.attack.elemental * level);
        attack.critical = baseStats.attack.critical + (statProgression.attack.critical * level);

        defense.physical = baseStats.defense.physical + (statProgression.defense.physical * level);
        defense.elemental = baseStats.defense.elemental + (statProgression.defense.elemental * level);
        defense.flinch = baseStats.defense.flinch - (statProgression.defense.flinch * level);

        modifiers.speed = baseStats.modifiers.speed + (statProgression.modifiers.speed * level);
        moveSpeed = baseStats.moveSpeed;
    }

    public bool Load(SaveData data)
    {
        if (data == null) return false;

        if (data.playerType != playerType) return false;
        level = data.playerLevel;
        m_element = (Element)data.playerElement;
        soulCount = data.soulCount;
        
        ChangeElement(m_element);
        exp.current = data.playerExp;
        exp.max = data.playerMaxExp;

        return true;
    }

    #endregion
}
