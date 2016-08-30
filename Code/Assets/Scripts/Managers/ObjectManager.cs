using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager manager;
    public static ObjectManager instance
    {
        get
        {
            if (manager)
                return manager;

            manager = FindObjectOfType<ObjectManager>();

            return manager;
        }
    }

    public GameObject soulObject;
    public GameObject playerExpGainObject;
    public GameObject playerHitObject;
    public GameObject enemyHitObject;

    [SerializeField]
    private Transform playerSpawnPosition;

    [SerializeField]
    private GameObject[] playerObjects;

    [SerializeField]
    private GameObject[] projectiles;

    [SerializeField]
    private GameObject[] explosions;

    [SerializeField]
    private EntityTextures[] playerTextures;

    [SerializeField]
    private EntityTextures[] enemyTextures;

    [SerializeField]
    private GameObject[] leapAbilityObjects;

    [SerializeField]
    private GameObject goeTelelportObject;
    [SerializeField]
    private GameObject goeDieEffectObject;

    public GameObject GoeTeleportObject
    {
        get
        {
            return goeTelelportObject;
        }
    }

    public GameObject GoeDieEffectObject
    {
        get
        {
            return goeDieEffectObject;
        }
    }

    public GameObject grimReaper;

    [SerializeField]
    private AudioClip battleMusic;

    private void Start()
    {
        if (SoundManager.instance)
            SoundManager.instance.SetBgMusic(battleMusic);

        if (!playerSpawnPosition) return;

        GameObject playerObj = (GameObject)Instantiate(playerObjects[PersistentInfo.selectedPlayer], playerSpawnPosition.position, playerSpawnPosition.rotation);
        CameraController cameraController = FindObjectOfType<CameraController>();
        cameraController.target = playerObj.transform;
    }

    public GameObject GetProjectile(Element element)
    {
        int index = (int)element;

        return projectiles[index];
    }

    public GameObject GetExplosion(Element element)
    {
        int index = (int)element;

        return explosions[index];
    }

    public GameObject GetLeapAbility(Element element)
    {
        int index = (int)element;

        return leapAbilityObjects[index];
    }

    public Texture GetPlayerTexture(PlayerType type, Element element)
    {
        int typeIndex = (int)type;

        return playerTextures[typeIndex][element];
    }

    public Texture GetEnemyTexture(EnemyType type, Element element)
    {
        int typeIndex = (int)type;

        return enemyTextures[typeIndex][element];
    }

}
