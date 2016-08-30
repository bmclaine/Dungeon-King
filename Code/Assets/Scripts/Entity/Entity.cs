using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    protected Vital health;
    protected Vital mana;
    protected AttackInfo attack;
    protected DefenseInfo defense;
    protected Interval exp;
    protected float moveSpeed = 5.0f;
    protected int level;
    protected StatModifiers modifiers;
    [HideInInspector] public EntityType entityType;

    protected Entity attackTarget;
    [SerializeField]
    protected Transform[] damagePositions;
    [SerializeField]
    private GameObject burnParticles;
    [SerializeField]
    private GameObject freezeParticles;

    public Vital Health
    {
        get
        {
            return health;
        }
    }
    public Vital Mana
    {
        get
        {
            return mana;
        }
    }
    public AttackInfo Attack
    {
        get
        {
            return attack;
        }
    }
    public DefenseInfo Defense
    {
        get
        {
            return defense;
        }
    }
    public Interval Exp
    {
        get
        {
            return exp;
        }
    }
    public Entity Attacktarget
    {
        get
        {
            return attackTarget;
        }

        set
        {
            attackTarget = value;
        }   
    }
    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    private List<BaseEffect> activeEffects = new List<BaseEffect>();
    protected List<Entity> attackTargets = new List<Entity>();

    public virtual void TakeDamage(float damage)
    {
        health.SubtractCurrent(damage);
    }

    public virtual void TakeDamage(ref HitInfo damage)
    {
        //damage.attackInfo.physical *= 1.5f;
        float difference = damage.attackInfo.physical * 0.3f;

        float physical = Random.Range(damage.attackInfo.physical - difference, damage.attackInfo.physical + difference);
        physical -= defense.physical;

        difference = damage.attackInfo.elemental * 0.3f;
        float elemental = Random.Range(damage.attackInfo.elemental - difference, damage.attackInfo.elemental + difference);

        damage.attackInfo.physical = physical;
        damage.attackInfo.elemental = elemental;

        health.SubtractCurrent(damage.totalDamage);

        if (damage.effects != null)
        {
            foreach (BaseEffectObject effect in damage.effects)
            {
                AddEffect(effect);
            }
        }
    }

    public virtual void AddEffect(BaseEffectObject effectObject)
    {
        if (effectObject == null) return;

        float chance = Random.Range(0.0f, 1.0f);

        if (chance > effectObject.chance) return;

        BaseEffect effect = effectObject.getEffect();

        BaseEffect sameEffect = activeEffects.Find(x => effect.id == x.id);

        if(sameEffect != null)
        {
            sameEffect.stack(effect);
            return;
        }

        effect.SetTarget(this);

        activeEffects.Add(effect);

        effect.Enter();
    }

    public void RemoveEffect(BaseEffect effect)
    {
        activeEffects.Remove(effect);
    }

    protected void UpdateEffects()
    {
        for (int i = 0; i < activeEffects.Count; ++i)
        {
            activeEffects[i].Execute();
        }
    }

    public void BoostAttack(float value)
    {
        modifiers.attack.physical += value;
        modifiers.attack.elemental += value;
    }

    public void ResetAttack()
    {
        modifiers.attack.physical = 1.0f;
        modifiers.attack.elemental = 1.0f;
    }

    public void BoostCritical(float value)
    {
        modifiers.attack.critical += value;
    }

    public void ResetCritical()
    {
        modifiers.attack.critical = 0.15f;
    }

    public void BoostDefense(float value)
    {
        modifiers.defense.physical += value;
        modifiers.defense.elemental += value;
    }

    public void ResetDefense(float value)
    {
        modifiers.defense.physical = 1.0f;
        modifiers.defense.elemental = 1.0f;
    }

    public void ModifyFlinch(float value)
    {
        defense.flinch += value;

        if (defense.flinch > 0.95f)
            defense.flinch = 0.95f;

        if (defense.flinch < 0.0f)
            defense.flinch = 0.0f;
    }

    public virtual void ResetHealth()
    {
        health.current = health.max;
    }

    public virtual void RestoreHealth(float value)
    {
        health.AddCurrent(value);
    }

    public virtual void RestoreMana(float value)
    {
        mana.AddCurrent(value);
    }

    public virtual void UseMana(float value)
    {
        mana.SubtractCurrent(value);
    }

    public virtual void ModifyMoveSpeed(float value)
    {
        modifiers.speed += value;

        if (value < 0.0f)
            ActivateFreeze();
        else
            DeactivateFreeze();
    }

    public virtual void ResetSpeed()
    {
        moveSpeed = 5.0f;
    }

    public virtual void AddExp(float value)
    {
        exp.current += value;

        if (exp.current >= exp.max)
            LevelUp();
    }

    public virtual void LevelUp()
    {
        exp.max *= 2.0f;
        exp.current = 0.0f;
        ++level;
    }

    public virtual void KnockBack()
    {
        return;
    }

    public virtual void Stun()
    {
    
    }

    public void ActivateBurn()
    {
        if (burnParticles) 
            burnParticles.SetActive(true);
    }

    public void ActivateFreeze()
    {
        if (freezeParticles)
            freezeParticles.SetActive(true);
    }

    public void DeactivateBurn()
    {
        if (burnParticles)
            burnParticles.SetActive(false);
    }

    public void DeactivateFreeze()
    {
        if (freezeParticles)
            freezeParticles.SetActive(false);
    }

    public Transform GetDamagePosition()
    {
        int index = Random.Range(0, damagePositions.Length - 1);
        return damagePositions[index];
    }

    public virtual void AddAttackTarget(Entity entity)
    {
        if (attackTargets.Contains(entity)) return;

        if (attackTargets.Count == 0)
            attackTarget = entity;

        attackTargets.Add(entity);
    }

    public virtual void RemoveAttackTarget(Entity entity)
    {
        attackTargets.Remove(entity);

        if(entity == attackTarget)
        {
            if (attackTargets.Count > 0)
                attackTarget = attackTargets[0];
            else
                attackTarget = null;
        }
    }
}
