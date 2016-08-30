using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour 
{
    public EnemySpawner spawner;

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player == null) return;

        spawner.Activate();

        gameObject.SetActive(false);
    }
}
