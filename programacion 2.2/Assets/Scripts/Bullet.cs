using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int danio = 1;
    public Transform bulletTransform;
    public float speed = 10f;

    private void Awake()
    {
        bulletTransform = this.GetComponent<Transform>();

    }

    void Update()
    {
        bulletTransform.position += bulletTransform.right * -speed * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            try { 
                collision.gameObject.GetComponent<PlayerController>().recibirDaño();
            }
            catch
            {
                collision.gameObject.GetComponent<TankController>().recibirDaño();
            }
        }
        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<Boss>().recibirDaño(this.danio);
        }
        Destroy(this.gameObject);
    }

}