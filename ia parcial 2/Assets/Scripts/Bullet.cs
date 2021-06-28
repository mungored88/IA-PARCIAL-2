using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Bullet : MonoBehaviour
{
    public int danio = 1;
    public Transform bulletTransform;

    //RouleteWheelSelection
    public Dictionary<string, float> velocity = new Dictionary<string, float>();
    RouleteWheelSelection _rws;

    private void Awake()
    {
        bulletTransform = this.GetComponent<Transform>();    
    }
    private void Start()
    {
        velocity.Add("speed1", 55);
        velocity.Add("speed2", 30);
        velocity.Add("speed3", 15);
        _rws = new RouleteWheelSelection();
        _rws.Velocity(velocity);
    }

    void Update()
    {
        Speed();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            try 
            {
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

    public void Speed()
    {

        bulletTransform.position += bulletTransform.right * -_rws.velocity * Time.deltaTime;
        Debug.Log("VELOCITY");
        Debug.Log(_rws.velocity);     
    }

}