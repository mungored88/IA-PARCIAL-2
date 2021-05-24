using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform StartPos;
    Vector3 NextPos;

    // Start is called before the first frame update
    void Start()
    {
        NextPos = StartPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pos1.position)
        {
            NextPos = pos2.position;
        }
        if (transform.position == pos2.position)
        {
            NextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = this.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;

        }
    }
}
