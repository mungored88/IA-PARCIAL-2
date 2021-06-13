using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifePot : potions
{
    public int hp = 1;
    public override void DarStats(Collider other)
    {
        other.gameObject.GetComponent<IPotionGrabber>().GetHp(this.hp);

    }
}
