using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : States<T>
{
	Bear _bear;
	PlayerModel _player;

	
	public AttackState(Bear b, PlayerModel p)
	{
		_bear = b;
		_player = p;
	}

	public override void Awake()
	{
		Debug.Log("Attack Awake");
		_bear.StopAllCoroutines();
	}

	public override void Execute()
	{
		Debug.Log("Attack");
		_bear.Attack();
		Sleep();
	}

	public override void Sleep()
	{
		Debug.Log("Attack Sleep");
		_bear.StartCoroutine(_bear.RunTreeTimer(_bear.questionInSight));
	}
}
