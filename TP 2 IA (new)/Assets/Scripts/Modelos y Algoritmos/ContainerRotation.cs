using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerRotation : MonoBehaviour
{
	public Vector3 rotation;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * speed);
    }
}
