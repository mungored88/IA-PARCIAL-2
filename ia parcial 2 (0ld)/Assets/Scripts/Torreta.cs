using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{

  public GameObject bullet;
  public float shootSpeed;
  public Transform bulletOrigin;
  public bool keepShooting = true;

  public virtual void Start()
  {
        Shoot();
  }
    protected virtual void Shoot()
    {
        StartCoroutine(StartShooting());
    }
    protected virtual void StopShoot()
    {
        keepShooting = false;
    }
  protected virtual IEnumerator StartShooting()
  {
    while (keepShooting) { 
        yield return new WaitForSeconds(shootSpeed);
        GameObject bulletInstance = Instantiate(bullet);
        bulletInstance.transform.forward = bulletOrigin.forward;
        bulletInstance.transform.position = bulletOrigin.position;
    }
        yield return null;
  }
}
