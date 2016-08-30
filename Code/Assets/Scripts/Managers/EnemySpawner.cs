using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnData> spawnData = new List<SpawnData>();
    [SerializeField]
    private bool activate = false;
    [SerializeField]
    private int enemyCount;
    [SerializeField]
    private int maxEnemyCount;
    private int spawnAmount;
    [SerializeField]
    private Transform anchorPosition;
    [SerializeField]
    private float roomSize;

    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }

        set
        {
            enemyCount = value;
            if (enemyCount == maxEnemyCount)
            {
                Deactivate();
            }
        }
    }

    public int MaxEnemyCount
    {
        get
        {
            return maxEnemyCount;
        }

        set
        {
            maxEnemyCount = value;
        }   
    }

    public GameObject[] barriers;

    private Player player;

    void Start()
    {
        Deactivate();
        SetUpEnemies();
        enemyCount = 0;

        player = EntityManager.instance.player;
    }

    void Update()
    {
        if(activate && spawnAmount < spawnData.Count)
            UpdateSpawn();
    }

    void UpdateSpawn()
    {
        for(int i = 0; i < spawnData.Count; ++i)
        {
            if (spawnData[i].spawned == true)
                continue;

            if (spawnData[i].timer > 0.0f)
                spawnData[i].timer -= 1.0f * Time.deltaTime;
            else
            {
                spawnData[i].spawnObject.SetActive(true);
                Enemy enemy = spawnData[i].spawnObject.GetComponent<Enemy>();
                enemy.SetStats(GetRandomLevel());
                spawnData[i].spawned = true;
                ++spawnAmount;
            }
        }
    }

    GameObject SpawnObject(GameObject obj, Transform point)
    {
        GameObject enemyObj = (GameObject)Instantiate(obj, point.position, point.rotation);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.Spawner = this;
        enemy.Attacktarget = EntityManager.instance.player;
        enemy.AnchorPosition = anchorPosition;
        enemy.WalkRadius = roomSize;
        enemyObj.SetActive(false);
        
        return enemyObj;
    }

    public void Activate()
    {
        activate = true;
        foreach(GameObject barrier in barriers)
        {
            barrier.SetActive(true);
            Renderer brenderer = barrier.GetComponent<Renderer>();
            brenderer.enabled = true;
        }

        float chance = Random.Range(0.0f, 1.0f);

        if (chance < 0.1f)
            CreateGrimReaper();
    }


    public void Deactivate()
    {
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(false);

            if (activate && spawnAmount < maxEnemyCount)
                UpdateSpawn();
        }
    }

    private void CreateGrimReaper()
    {
        if (!ObjectManager.instance) return;

        Vector3 position = Random.insideUnitSphere * roomSize + anchorPosition.position;

        GameObject grimReaperObj = (GameObject)Instantiate(ObjectManager.instance.grimReaper, position, Quaternion.identity);
        GrimReaper grimReaper = grimReaperObj.GetComponent<GrimReaper>();
        grimReaper.player = EntityManager.instance.player;
    }

    private void SetUpEnemies()
    {
        for(int i = 0; i < spawnData.Count; ++i)
        {
            spawnData[i].spawnObject = SpawnObject(spawnData[i].spawnObject, spawnData[i].spawnPoint);
        }
    }

    private int GetRandomLevel()
    {
        if (!player)
            player = EntityManager.instance.player;

        if (!player) return 1;

        int level = Random.Range(player.Level - 1, player.Level + 1);

        if (level <= 0)
            level = 3;

        return level; 
    }
}

