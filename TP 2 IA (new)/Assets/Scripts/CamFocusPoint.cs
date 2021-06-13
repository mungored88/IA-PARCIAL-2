using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFocusPoint : MonoBehaviour
{
    public Transform player;
    public Transform cam;
    public float dist;
    // Start is called before the first frame update
    void Start()
    {
        dist = (player.position - cam.position).magnitude;
        cam.transform.localPosition = player.right * -1 * dist + new Vector3(0,2,0);
        cam.transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}
