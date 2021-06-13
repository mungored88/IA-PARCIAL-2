using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PatrolState<T> : States<T>
{
	Enemy _enemy;

	public PatrolState(Enemy enemy)
	{
		_enemy = enemy;
	}

	public override void Awake()
	{

	}

	public override void Execute()
	{
		_enemy.Patrol();
	}

	public override void Sleep()
	{

	}
}
