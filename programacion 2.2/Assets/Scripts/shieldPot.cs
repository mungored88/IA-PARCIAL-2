using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldPot : potions
{
    public int shield = 15;
    public override void DarStats(Collider other)
    {
        other.gameObject.GetComponent<IPotionGrabber>().GetShield(this.shield);

    }
}
