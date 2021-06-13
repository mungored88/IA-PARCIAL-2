using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerUp : PowerUp
{
    public int SpeedUpProbability = 50;
    public int ShieldProbability = 30;
    public int MagnetProbability = 20;

    RouletteWheel roulette = null;
    Dictionary<string, int> powerUpsKeys = null;

    private void OnTriggerEnter(Collider other)
    {
        executeRoulette();
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            NotifyObserver(executeRoulette());
			gameObject.SetActive(false);
        }
    }


    public string executeRoulette()
    {
        if (powerUpsKeys == null)
        {
            powerUpsKeys = new Dictionary<string, int>
            {
                { "Speed", SpeedUpProbability },
                { "Shield", ShieldProbability },
                { "Magnet", MagnetProbability }
            };
            roulette = new RouletteWheel();
        }

        return roulette.Execute(powerUpsKeys);
    }
}
