using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : States<T>
{
	BombaLoca _bomba;
	Animator _anim;

	public IdleState(BombaLoca bomb)
	{
		_bomba = bomb;
	}

	public override void Execute()
	{
		_bomba.Idle();
		Debug.Log("Execute de Idle");
	}

	public override void Sleep()
	{
		Debug.Log("Sleep de Idle");
	}
}
