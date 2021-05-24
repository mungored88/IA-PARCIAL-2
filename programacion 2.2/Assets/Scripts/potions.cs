using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class potions : MonoBehaviour
{
    public GameObject sonido;
    public abstract void DarStats(Collider Player);
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        DarStats(other);
        Destroy(Instantiate(sonido), 3f);
        Destroy(this.gameObject);

    }
}
