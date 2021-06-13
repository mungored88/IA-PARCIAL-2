using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public Transform player;
    public float posX;
    public float posY;
    public float speed;
    public Vector3 vecDistance;
    private Transform _cam;
    // Start is called before the first frame update
    void Start()
    {
        _cam = transform;
    }
     
    // Update is called once per frame
    void Update()
    {
        posX = speed * Input.GetAxis("Mouse X");
        posY = speed * Input.GetAxis("Mouse Y");
        _cam.Translate(new Vector3(-posX,-posY,0));
        _cam.LookAt(player);
    }
}
