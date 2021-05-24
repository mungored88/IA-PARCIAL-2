using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    public AudioSource audios;
    public potions[] potions;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            audios.Play(0);
            if(Random.Range(0,3)>1)
            {
                potions potionInstance = Instantiate(potions[Random.Range(0, potions.Length)]);
                potionInstance.transform.position = this.transform.position;
            }
            Destroy(this.gameObject, 0.7f);
        }
    }
}
