using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo : ScriptableObject
{
    new public string name;
    public Vital health;
    public Vital mana;
    public AttackInfo attack;
    public DefenseInfo defense;
    public StatModifiers modifiers;
    public float moveSpeed;
}
