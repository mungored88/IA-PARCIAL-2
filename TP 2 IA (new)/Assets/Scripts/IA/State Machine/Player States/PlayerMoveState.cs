using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState<T> : States<T>
{
	Player _player;

	public PlayerMoveState(Player p)
	{
		_player = p;
	}

	public override void Execute()
	{
		_player.Move();
	}
}
