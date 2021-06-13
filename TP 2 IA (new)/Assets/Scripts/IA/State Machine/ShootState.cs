using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState<T> : States<T>
{
	Monkey _enemy;
	Vector3 _direction;
	BulletSpawn _bullet;
	Transform _target;

	public ShootState(Monkey enemy, BulletSpawn bullet, Transform target)
	{
		_enemy = enemy;
		_bullet = bullet;
		_target = target;
	}

	public override void Awake()
	{
		
	}

	public override void Execute()
	{
		_enemy.Shoot();
	}

	public override void Sleep()
	{
		
	}
}
