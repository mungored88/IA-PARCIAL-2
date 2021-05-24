using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    [SerializeField] private KeyType keytype;

    public key(KeyType keytype)
    {
        this.keytype = keytype;
    }

    public enum KeyType 
    {
        red,
        yellow,
        blue,
        green
    }
    public KeyType GetKeyType()
    {
        return keytype;
    }

    public void Update()
    {
        transform.Rotate(new Vector3(0,50*Time.deltaTime,0));
    }
}
