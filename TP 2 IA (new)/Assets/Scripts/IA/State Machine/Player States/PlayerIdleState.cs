using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState<T> : States<T>
{
	
	Player _player;
	Renderer _myRender;
	Color _normalColor;
	public PlayerIdleState(Player p, Renderer renderer)
	{
		_player = p;
		_myRender = renderer;
		_normalColor = renderer.material.color;
	}


	public override void Execute()
	{
		_myRender.material.color = Random.ColorHSV();
	}

	public override void Sleep()
	{
		_myRender.material.color = _normalColor;
	}
	
}
