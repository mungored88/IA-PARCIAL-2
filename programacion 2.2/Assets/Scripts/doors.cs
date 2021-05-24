using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    [SerializeField] private key.KeyType keytype;
    public Animator DoorAnim;

    public void Start()
    {
        DoorAnim = this.GetComponent<Animator>();
    }

    public doors(key.KeyType keytype)
    {
        this.keytype = keytype;
    }

    public AudioSource doorsound;
    public key.KeyType GetKeyType()
    {
        return keytype;
    }
    public void OpenDoor()
    {
        doorsound.Play(0);
        StartCoroutine(abrirPuerta());
        
    }

    public IEnumerator abrirPuerta()
    {
        DoorAnim.SetTrigger("opendoor");
        
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

}
