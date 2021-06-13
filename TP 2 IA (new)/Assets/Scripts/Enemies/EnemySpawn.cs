using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum CurrentEnemy
    {
        Monkey,
        Bear,
        Bird
    }

    public Enemy prefab;
    public int stock;
    public CurrentEnemy EnemyContained;

    public ObjectPool<Enemy> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(EnemyFactory, Enemy.TurnOnEnemy, Enemy.TurnOffEnemy, stock, true);
    }

    public Enemy EnemyFactory()
    {
        return Instantiate(prefab);
    }

    public void ReturnEnemyToPool(Enemy enemy)
    {
        enemyPool.ReturnObjectoToPool(enemy);
    }
}
