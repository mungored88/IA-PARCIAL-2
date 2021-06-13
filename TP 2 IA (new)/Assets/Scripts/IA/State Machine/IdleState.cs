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
		_enemy.StopAllCoroutines();

	}

	public override void Execute()
	{

		_enemy.StartCoroutine(_enemy.RunTreeTimer(_enemy.questionInSight));
	}

	public override void Sleep()
	{
		Debug.Log("Idle Sleep");
	}
}
