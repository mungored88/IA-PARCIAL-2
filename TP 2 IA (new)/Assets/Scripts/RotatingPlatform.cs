using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float speed;
    float time;
    private void Start()
    {
        if (speed == 0)
            speed = 1;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, time/speed));
    }
}
