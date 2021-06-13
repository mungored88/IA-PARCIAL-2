using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState<T> : States<T>
{
	Enemy _enemy;
	Vector3 _direction;
	
	public ShootState(Enemy enemy, Vector3 direction)
	{
		_enemy = enemy;
		_direction = direction;
	}

	public override void Awake()
	{
		_enemy.chasing = true;
	}

	public override void Execute()
	{
		_enemy.Shoot(_direction, true);
	}

	public override void Sleep()
	{
		_enemy.Shoot(_direction, false);
		_enemy.chasing = false;
	} 
}
