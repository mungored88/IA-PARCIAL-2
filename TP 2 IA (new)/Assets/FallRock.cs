using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRock : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * -1 * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerModel player = collision.gameObject.GetComponent<PlayerModel>();
        if (player != null)
            player.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerModel player = collision.gameObject.GetComponent<PlayerModel>();
        if (player != null)
            player.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.DetachChildren();
        RockTeleporter tp = other.GetComponent<RockTeleporter>();
        if (tp != null)
            tp.Teleport(transform);
    }
}
