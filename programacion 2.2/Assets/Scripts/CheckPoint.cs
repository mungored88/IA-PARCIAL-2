using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CheckPoint : MonoBehaviour
{

    [SerializeField] public CheckPointOBJ lastCheck;

    private void Awake()
    {
        lastCheck = new CheckPointOBJ();
    }
    public CheckPointOBJ getLastCheckPoint()
    {
        return lastCheck;
    }

    public void setCheckPoint(Transform tran)
    {  
        lastCheck.lastCheckPos = tran.position;
        lastCheck.lastCheckRot = tran.rotation;
        lastCheck.scene = SceneManager.GetActiveScene().name;
    }
}
[Serializable]
public class CheckPointOBJ
{
    [SerializeField] public Vector3 lastCheckPos;
    [SerializeField] public Quaternion lastCheckRot;
    [SerializeField] public String scene;
}

