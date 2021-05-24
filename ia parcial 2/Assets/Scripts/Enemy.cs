using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    protected GameObject Player;
    [SerializeField] protected float distanceToPlayer;
    [SerializeField] protected float attackDistance;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.GetComponent<PlayerController>().recibirDaño();
        }
    }

    public IEnumerator hacerDañoEnSegundos(float segs)
    {

        yield return new WaitForSeconds(segs);
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= attackDistance)
        {
            Player.GetComponent<PlayerController>().recibirDaño();
        }
        yield return null;
    }

}
