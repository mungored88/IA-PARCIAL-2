using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPotion : MonoBehaviour
{
    public int speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
