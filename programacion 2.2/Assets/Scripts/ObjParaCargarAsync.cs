using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ObjParaCargarAsync : MonoBehaviour
{
    public int nivelACargar;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
