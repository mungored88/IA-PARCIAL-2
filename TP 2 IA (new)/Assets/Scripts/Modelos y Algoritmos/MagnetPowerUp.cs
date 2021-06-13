using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerUp : PowerUp
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerModel>() != null)
        {
            Debug.Log("magnet agarrado");
            NotifyObserver("Magnet");
			gameObject.SetActive(false);
		}
    }
}
