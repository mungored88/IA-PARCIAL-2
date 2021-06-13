using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent<PlayerModel>(out PlayerModel player))
			player.GetHit();
	}
}
