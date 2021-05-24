using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContadorDeVidas : MonoBehaviour
{
    public int Life;
    public int NumberOfHeart;
    public Image[] hearts;
    public Sprite fulHeart;
    public Sprite emptyHeart;


    public void Update()
    {
        
        if (Life > NumberOfHeart)
        {
            Life = NumberOfHeart;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Life)
            {
                hearts[i].sprite = fulHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < NumberOfHeart)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        if (Life <= 0)
        {
            Die();
        }
    }

    void Die()
    {
       GameObject.Destroy(this.gameObject);
       SceneManager.LoadScene("04 lose");
    }

}
