using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject splashScreen;
    private void Start()
    {
        if (splashScreen != null)
        {
            splashScreen.SetActive(true);
            StartCoroutine(SplashTimer());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && splashScreen != null)
            SceneManager.LoadScene("Menu");
    }
    public void Play()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator SplashTimer()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("Menu");
    }


}
