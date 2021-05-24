using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyholder : Sounds
{
    private List<key.KeyType> keyList;
    public Sounds playersound;
    public ContadorDeLlaves contadorLLaves;
    private void Awake()
    {
        keyList = new List<key.KeyType>();
        contadorLLaves = this.gameObject.GetComponent<ContadorDeLlaves>();

    }

    public void Start()
    {
        int cant_llavesR = contadorLLaves.R;
        int cant_llavesB = contadorLLaves.B;
        int cant_llavesG = contadorLLaves.G;
        int cant_llavesY = contadorLLaves.Y;

        for (int i = 0; i < cant_llavesR; i++)
        {
            keyList.Add(key.KeyType.red);
        }
        for (int i = 0; i < cant_llavesB; i++)
        {
            keyList.Add(key.KeyType.blue);
        }
        for (int i = 0; i < cant_llavesG; i++)
        {
            keyList.Add(key.KeyType.green);
        }
        for (int i = 0; i < cant_llavesY; i++)
        {
            keyList.Add(key.KeyType.yellow);
        }
    }
    public void AddKey(key.KeyType keyType)
    {
        //Debug.Log("agarro llave " + keyType);
        contadorLLaves.agarrarLLave(keyType);
        playersound.SoundPlay(playersound.clips[0]);
        keyList.Add(keyType);
    }
    public void removeKey(key.KeyType key)
    {
        contadorLLaves.usarLLave(key);

        keyList.Remove(key);
    }
    public bool ContainKey(key.KeyType keytype)
    {
        return keyList.Contains(keytype);
    }
    private void OnTriggerEnter(Collider collider)
    {

        key key = collider.GetComponent<key>();
        if(key!=null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }
        doors keyDoor = collider.GetComponent<doors>();
        if (keyDoor!=null)
        {
            if(ContainKey(keyDoor.GetKeyType()))
            {
                removeKey(keyDoor.GetKeyType());
                keyDoor.OpenDoor();
            }
        }
    }
}
