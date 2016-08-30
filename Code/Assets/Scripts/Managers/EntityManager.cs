using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour 
{
    private static EntityManager manager;
    public static EntityManager instance
    {
        get
        {
            if (manager)
                return manager;

            manager = FindObjectOfType<EntityManager>();

            if (manager == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<EntityManager>();
                go.name = "Entity Manager";
                manager = go.GetComponent<EntityManager>();
            }

            return manager;
        }
    }

    public List<Enemy> enemies = new List<Enemy>();
    public List<PenguinKnight> penguinKnights = new List<PenguinKnight>();
    public Player player;

    public bool AddEnemy(Enemy _enemy)
    {
        if (enemies.Contains(_enemy))
            return false;

        enemies.Add(_enemy);

        return true;
    }

    public void RemoveEnemy(Enemy _enemy)
    {
        if(player)
            player.RemoveAttackTarget(_enemy);

        foreach (PenguinKnight knight in penguinKnights)
        {
            knight.RemoveAttackTarget(_enemy);
        }

        enemies.Remove(_enemy);
    }

    public void AddPlayer(Player _player)
    {
        if (player != null)
            return;

        foreach (Enemy enemy in enemies)
            enemy.AddAttackTarget(player);

        player = _player;
    }

    public void RemovePlayer()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.RemoveAttackTarget(player);
        }
    }

    public void AddPenguinKnight(PenguinKnight knight)
    {
        if (penguinKnights.Contains(knight))
            return;

        penguinKnights.Add(knight);
    }

    public void RemovePenguinKnight(PenguinKnight knight)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.RemoveAttackTarget(knight);
        }

        penguinKnights.Remove(knight);
    }

    public void RemoveAllPenguinKnights()
    {

        foreach(Enemy enemy in enemies)
        {
            for(int i = 0; i < penguinKnights.Count; ++i)
            {
                enemy.RemoveAttackTarget(penguinKnights[i]);
            }
        }

        for(int i = 0; i < penguinKnights.Count; ++i)
        {
            Destroy(penguinKnights[i].gameObject);
        }

        penguinKnights.Clear();
    }
}
