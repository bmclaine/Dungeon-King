using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct AttackInfo
{
    public float physical;
    public float elemental;
    [Range(0.0f, 1.0f)]
    public float critical;

    public void AddCritical(float value)
    {
        critical += value;

        if (critical > 1.0f)
            critical = 1.0f;
    }

    public void SubtractCritical(float value)
    {
        critical -= value;
        if (critical < 0.0f)
            critical = 0.0f;
    }
}

[System.Serializable]
public struct HitInfo
{
    public AttackInfo attackInfo;
    public List<BaseEffectObject> effects;
    public Element element;

    public float totalDamage
    {
        get
        {
            float value = attackInfo.elemental + attackInfo.physical;
            if (value < 1)
                return 1.0f;

            return value;
        }
    }

    public HitInfo(AttackInfo info)
    {
        effects = new List<BaseEffectObject>();
        attackInfo = info;

        element = Element.None;
    }
}

[Serializable]
public struct DefenseInfo
{
    public float physical;
    public float elemental;
    [Range(0.0f, 1.0f)]
    public float flinch;
}

[Serializable]
public struct Vital
{
    //Used for stuff like health and mana
    public float current;
    public float max;
    public float regen;

    public float percent
    {
        get
        {
            return current / max;
        }
    }

    public void AddCurrent(float value)
    {
        current += value;
        if (current > max)
            current = max;
        if (current < 0.0f)
            current = 0.0f;
    }

    public void SubtractCurrent(float value)
    {
        current -= value;
        if (current < 0.0f)
            current = 0.0f;
    }

    public void AddMax(float value)
    {
        max += value;
    }

    public void MaxOut()
    {
        current = max;
    }

    public void Regenerate()
    {
        AddCurrent(regen * Time.deltaTime);
    }
}

[Serializable]
public struct Interval
{
    public float current;
    public float max;

    public float percent
    {
        get
        {
            return current / max;
        }
    }
}

public enum Element
{
    None = 0,
    Fire = 1,
    Water = 2,
    Wind = 3,
    Light = 4,
    Dark = 5
}

public enum PotionType
{
    None = 0,
    Health = 1,
    Mana = 2,
    Speed = 3,
    Might = 4,
    Toughness = 5
}

public enum WeaponType
{
    None = 0,
    Fire = 1,
    Water = 2,
    Wind = 3,
    Light = 4,
    Dark = 5
}

public enum PlayerType
{
    Archangel = 0,
    DeathKnight = 1,
    DragonSlayer = 2,
    None = 3
}

public enum PlayerState
{
    Idle = 0,
    Move = 1,
    Attack = 2,
    Projectile = 3,
    Ability = 4,
    Block = 5,
    Damage = 6,
    Die = 7,
    ChangeElement = 8,
    Stun = 9,
    Knockback = 10,
    Falling = 11
}

public enum EnemyState
{
    Idle = 0,
    Move = 1,
    Attack = 2,
    Projectile = 3,
    Ability = 4,
    Block = 5,
    Damage = 6,
    Die = 7,
    Pursue = 8,
    Knockback = 9,
    Summon = 10,
    Skill = 11
}

public enum CompanionState
{
    Idle = 0,
    Pursuit = 2,
    Attack = 3,
    Damage = 4,
    Die = 7,
    Knockback = 8
}

public enum EntityType
{
    None,
    Player = 1,
    Enemy = 2,
    Companion = 3
}

public enum EnemyType
{
    Skeleton = 0,
    Salamander = 1,
    Inferno = 2,
    Goe = 3,
    Hellhound = 4,
    Lich = 5,
    AncientGiant = 6,
    Yeti = 7,
    Dragon = 8,
    Ifrit = 9,
    LegendaryDragon = 10,
    OtherworldlyDragon = 11,
    Chimera = 12,
    None = 13
}

public enum ChestType
{
    Small = 0,
    Medium = 1,
    Large = 2,
    Epic = 3
}

[Serializable]
public struct Ability
{
    public string name;
    public float manaCost;
    public Vital coolDown;
    public GameObject abilityObject;
    public Transform abilityLocation;
    public bool unlocked;
}

[Serializable]
public struct StatModifiers
{
    public float speed;
    public AttackInfo attack;
    public DefenseInfo defense;

    public static StatModifiers one
    {
        get
        {
            StatModifiers m = new StatModifiers();
            m.speed = 1.0f;
            m.attack.physical = 1.0f;
            m.attack.elemental = 1.0f;
            m.defense.physical = 1.0f;
            m.defense.elemental = 1.0f;

            return m;
        }
    }
}

[Serializable]
public class SpawnData
{
    public string name;
    public float timer;

    public GameObject spawnObject;
    public Transform spawnPoint;
    public Rarity rarity;
    public bool spawned = false;
}

public enum Rarity
{
    Common = 10,
    UnCommon = 45,
    Rare = 75,
    Legendary = 90
}

public enum PauseState
{
    Pause = 0,
    Play = 1
}
