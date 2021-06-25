using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackState<T> : States<T>
{
	BombaLoca _bomba;
	Animator _anim;
	AudioSource _aud;
	

	public AtackState(BombaLoca bomb, Animator animator, AudioSource audio)
	{
		_bomba = bomb;
		_anim = animator;
		_aud = audio;		
	}
	public override void Awake()
	{
		_bomba.Atack();
	}
	public override void Execute()
	{
		
		Debug.Log("Execute de Atack");
	}
	public override void Sleep()
	{
		Debug.Log("Sleep de Atack");
	}
}
