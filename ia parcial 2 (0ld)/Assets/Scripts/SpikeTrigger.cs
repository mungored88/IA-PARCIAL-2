using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{

    public Animator anim;
    public AudioSource sound;

    private void OnCollisionEnter(Collision other)
    {
        anim.SetTrigger("EnterPlayer");
        sound.PlayDelayed(0.3f);
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().recibirDaño();
        }
    }

}
