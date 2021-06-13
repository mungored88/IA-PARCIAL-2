using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerModel>() != null)
        {
            Debug.Log("speed agarrado");
            NotifyObserver("Speed");
			gameObject.SetActive(false);
		}
    }
}
