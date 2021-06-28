using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activacionDeTanke : MonoBehaviour
{

    public GameObject camera1;
    public GameObject camera2;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            camera2.SetActive(true);
            camera1.SetActive(false);
            GetComponent<TankController>().enabled = true;
            GetComponent<ShootWithTank>().enabled = true;
            GetComponent<AudioListener>().enabled = true;

            GetComponent<JoystickController>().SetTankButton();

            
        }
    }
}
