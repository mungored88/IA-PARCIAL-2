using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hackactive : MonoBehaviour
{
    public GameObject hack;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Alpha0)) hack.gameObject.SetActive(true);
    }
}
