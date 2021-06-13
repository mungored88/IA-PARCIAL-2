using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, IObserver
{
    public EnemySpawn.CurrentEnemy EnemyToSpawn;

    [Header("Line of Sight")]
    public float range = 10f;
    public float angle = 60f;

    private PlayerModel _player;
    private Enemy _enemy;
    private EnemySpawn _spawner;
    private bool _hasSpawned = false;

    private delegate void Execute();
    private Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    private void Awake()
    {
        _actionsDic.Add("OnEnemyDeath", OnDeath);
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerModel>();

        var enemyPools = FindObjectsOfType<EnemySpawn>();
        foreach (var pool in enemyPools)
        {
            if (pool.EnemyContained == EnemyToSpawn)
            {
                _spawner = pool;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ontriggerenter");
        if (other.TryGetComponent<PlayerModel>(out var target))
            SpawnWhenClose();
    }

    private void SpawnWhenClose()
    {
        if (_hasSpawned) return;
        if (_player == null) return;
        if (_spawner == null) return;

        _enemy = _spawner.enemyPool.PickObjectFromPool();
        _enemy.transform.position = transform.position;
        _enemy.transform.rotation = transform.rotation;
		if(_enemy.LineOfSight != null)
		{
			_enemy.LineOfSight.range = range;
			_enemy.LineOfSight.angle = angle;
		}
		
        _enemy.Subscribe(this);
        _hasSpawned = true;
    }

    private void OnDeath()
    {
        _spawner.ReturnEnemyToPool(_enemy);
    }

    public void OnAction(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
    }
}
