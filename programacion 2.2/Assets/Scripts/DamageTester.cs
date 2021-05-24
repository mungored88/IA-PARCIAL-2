using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TESTER  DamageTester  CLASS
 * Clase creada para testear cuando se hace daño
 */
public class DamageTester : MonoBehaviour
{
    public GameObject damageTesterObj;
    public IEnumerator damageTesterNumerator() {
        damageTesterObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageTesterObj.SetActive(false);
        yield break;
    }

    internal void daño()
    {
        StartCoroutine(damageTesterNumerator());
    }
}
