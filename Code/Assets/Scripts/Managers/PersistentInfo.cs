using UnityEngine;
using System.Collections;

public class PersistentInfo : MonoBehaviour 
{
    private static PersistentInfo info;
    public static PersistentInfo instance
    {
        get
        {
            if (info)
                return info;

            info = FindObjectOfType<PersistentInfo>();

            return info;
        }
    }

    public static SaveData saveData;
    public static string slotName = "";
    public static string levelToLoad = string.Empty;
    public static int selectedPlayer;
    public string defaultLevelName;

    public static int screenWidth = -1;
    public static int screenHeight = -1;

    public Sprite[] characterImages;

    private AsyncOperation async;

    [SerializeField]
    private EnemyInfo[] enemyInfo;

    [SerializeField]
    private EnemyInfo[] enemyStatProgression;

    public EnemyInfo getEnemy(EnemyType type)
    {
        int index = (int)type;
        return enemyInfo[index];
    }

    public EnemyInfo getEnemyStatProgression(EnemyType type)
    {
        int index = (int)type;
        return enemyStatProgression[index];
    }

	void Awake () 
    {
        Time.timeScale = 1.0f;

        if (screenHeight < 0)
            screenHeight = Screen.height;

        if (screenWidth < 0)
            screenWidth = Screen.width;

        if (slotName == string.Empty)
            slotName = "slot 1";
	}

    public static void LoadLevel(string level)
    {
        levelToLoad = level;
        Application.LoadLevel("Loading Screen");
    }
}
