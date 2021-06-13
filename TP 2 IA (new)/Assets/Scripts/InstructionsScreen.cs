using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            SceneManager.LoadScene("LVL IA");
        }

    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("LVL IA");
    }
}
