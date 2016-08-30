using UnityEngine;
using UnityEditor;

public class ScriptableObjectCreator : MonoBehaviour 
{
    [MenuItem("Assets/Create/Effects/DOT Effect")]
    public static void CreateDOT()
    {
        ScriptableObjectUtility.CreateAsset<DOTEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Speed Multiplier")]
    public static void CreateSpeedMultiplierEffect()
    {
        ScriptableObjectUtility.CreateAsset<SpeedMultiplierEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Health Restore")]
    public static void CreateHealthRestoreEffect()
    {
        ScriptableObjectUtility.CreateAsset<HealthRestoreEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Mana Restore")]
    public static void CreateManaRestoreEffect()
    {
        ScriptableObjectUtility.CreateAsset<ManaRestoreEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Attack Boost")]
    public static void CreateAttackBoostEffect()
    {
        ScriptableObjectUtility.CreateAsset<AttackBoostEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Defense Boost")]
    public static void CreateDefenseBoostEffect()
    {
        ScriptableObjectUtility.CreateAsset<DefenseBoostEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Critical Boost")]
    public static void CreateCritBoostEffect()
    {
        ScriptableObjectUtility.CreateAsset<CritBoostEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Instant Death")]
    public static void CreateInstantDeathEffect()
    {
        ScriptableObjectUtility.CreateAsset<InstantDeathEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Divine Light")]
    public static void CreateDivineLightEfffect()
    {
        ScriptableObjectUtility.CreateAsset<DivineLightEffectObject>();
    }

    [MenuItem("Assets/Create/Effects/Flinch Modifier")]
    public static void CreatFlinchModifierEffect()
    {
        ScriptableObjectUtility.CreateAsset<FlinchEffectObject>();
    }

    [MenuItem("Assets/Create/Entity Stat/Enemy")]
    public static void CreateEnemyInfo()
    {
        ScriptableObjectUtility.CreateAsset<EnemyInfo>();
    }

    [MenuItem("Assets/Create/Entity Stat/Player")]
    public static void CreatePlayerInfo()
    {
        ScriptableObjectUtility.CreateAsset<PlayerInfo>();
    }

    [MenuItem("Assets/Create/Entity Texture")]
    public static void CreateEntityTexture()
    {
        ScriptableObjectUtility.CreateAsset<EntityTextures>();
    }

    [MenuItem("Tools/Delete Player Prefs")]
    public static void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
