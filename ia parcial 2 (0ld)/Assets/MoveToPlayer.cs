using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float speed=10f;
    public Pathfinding Pf;
    // Update is called once per frame
    void Update()
    {

        float step = speed * Time.deltaTime; 
        try { 
            transform.position = Vector3.MoveTowards(transform.position, Pf.EnemyPath[1].vPosition, step);
            transform.LookAt(Pf.EnemyPath[1].vPosition);
        }
        catch
        {
            return;
        }
    }
}
