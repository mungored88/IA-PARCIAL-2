using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWithTank : MonoBehaviour
{

    public Transform bulletOrigin;
    public GameObject bullet;
    public float tiempoEntreDisparos = 0.5f;
    

    public void Update()
    {
        tiempoEntreDisparos -= Time.deltaTime;
    }

    public void Disparar()
    {
        if (tiempoEntreDisparos > 0) return;
        GameObject bulletInstance = Instantiate(bullet);
        bulletInstance.GetComponent<Bullet>().danio = this.GetComponent<TankController>().danioDeTanke;
        bulletInstance.transform.forward = this.transform.right;
        bulletInstance.transform.position = bulletOrigin.position;
        tiempoEntreDisparos = 0.5f;
    }
}
