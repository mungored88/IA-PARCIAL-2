using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState<T> : States<T>
{
	BombaLoca _bomba;
	Animator _anim;
	
	public WalkState(BombaLoca bomb, Animator animator)
	{
		_bomba = bomb;
		_anim = animator;
	}
	public override void Execute()
	{
		_bomba.Walk();
		
		Debug.Log("Execute de Walk");
	}
	public override void Sleep()
	{
		Debug.Log("Sleep de Iddle");
	}
}
