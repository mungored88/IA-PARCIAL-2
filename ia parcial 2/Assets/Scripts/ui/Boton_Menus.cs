using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boton_Menus : MonoBehaviour
{
    public void ClickInBoton(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Exit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void ReserPlayerPref()
    {

        PlayerPrefs.DeleteAll();
    }
}

