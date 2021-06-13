using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : States<T>
{
	Enemy _enemy;

	public IdleState(Enemy e)
	{
		_enemy = e;
	}
	 
	public override void Awake()
	{
		Debug.Log("Idle Awake");
		_enemy.isIdle = true;
	}

	public override void Execute()
	{
		_enemy.Idle();
		if (_enemy.inSight)
			_enemy.RunTree();
	}

	public override void Sleep()
	{
		_enemy.transform.rotation = Quaternion.Lerp(_enemy.transform.rotation, Quaternion.identity, 10f);
		_enemy.isIdle = false;
		Debug.Log("Idle Sleep");
	}
}
