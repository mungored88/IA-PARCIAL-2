using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContadorDeLlaves : MonoBehaviour
{
    [Header("Variables")]
    public int R;
    public int B;
    public int G;
    public int Y;

    [Header("UI")]
    public Text Rt;
    public Text Bt;
    public Text Gt;
    public Text Yt;


    private void Update()
    {
        Rt.text = R.ToString();
        Bt.text = B.ToString();
        Gt.text = G.ToString();
        Yt.text = Y.ToString();
    }
    public void agarrarLLave(key.KeyType llave)
    {
        switch (llave) {
            case key.KeyType.red:
                R += 1;
                break;
            case key.KeyType.blue:
                B += 1;
                break;
            case key.KeyType.green:
                G += 1;
                break;
            case key.KeyType.yellow:
                Y += 1;
                break;
        }
    } 
    public void usarLLave(key.KeyType llave)
    {
        switch (llave) {
            case key.KeyType.red:
                R -= 1;
                break;
            case key.KeyType.blue:
                B -= 1;
                break;
            case key.KeyType.green:
                G -= 1;
                break;
            case key.KeyType.yellow:
                Y -= 1;
                break;
        }
    }

}
