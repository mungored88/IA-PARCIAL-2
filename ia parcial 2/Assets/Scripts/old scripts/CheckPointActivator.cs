using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointActivator : MonoBehaviour
{
    public GameObject banderitaAsociada;
    public AudioSource playersound;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().guardarCheckPoint();
            playersound.Play(0);
            banderitaAsociada.SetActive(true);
        }
    }

 
}
