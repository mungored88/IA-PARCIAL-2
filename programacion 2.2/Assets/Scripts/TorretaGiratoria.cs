using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaGiratoria : Torreta
{

    public Transform character;
    public float rotationSpeed = 10f;

    void Awake()
    { 
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(character.position - transform.position), rotationSpeed * Time.deltaTime);
        rotation.z = 0;
        rotation.x = 0;
        this.transform.rotation = rotation;

    }


}
