using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player;
    public float distance;
    public Vector3 vecDistance;

	public float speedH = 2f;
	public float speedV = 2f;

	private float rotationY = 0f;
	private float rotationX = 0f;
     
    
    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

	private void LateUpdate()
	{
		rotationY += speedH * Input.GetAxis("Mouse X");
		rotationX -= speedV * Input.GetAxis("Mouse Y");

		transform.eulerAngles = new Vector3(rotationX, rotationY, 0);
	}

	public void CameraMovement()
    {
		
        transform.position = player.position + vecDistance;
    }

}
