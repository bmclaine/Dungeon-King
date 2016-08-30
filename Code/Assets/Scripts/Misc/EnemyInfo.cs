using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyInfo : ScriptableObject
{
    new public string name;
    public Vital health;
    public Vital mana;
    public AttackInfo attack;
    public DefenseInfo defense;
    public float moveSpeed;
    public float exp;
    public StatModifiers modifiers;
}
