using UnityEngine;
using System.Collections;

public class EnemySpawnerTrigger : MonoBehaviour
{
    public bool usableTrigger;
    public EnemySpawner spawner;

    // Use this for initialization
    void Start()
    {
        usableTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player == null) return;
        if (usableTrigger)
        {
            usableTrigger = false;

            spawner.Activate();
        }
    }
}
