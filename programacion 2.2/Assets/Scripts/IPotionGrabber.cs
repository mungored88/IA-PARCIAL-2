using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPotionGrabber
{
    void GetHp(int hp);
    void GetShield(int shield);
    void GetPwr(int pwr);
}
