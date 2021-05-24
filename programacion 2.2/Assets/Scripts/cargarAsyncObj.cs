using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cargarAsyncObj : MonoBehaviour
{
    public string nivelACargar;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        int lvl = FindObjectOfType<ObjParaCargarAsync>().nivelACargar;
        switch (lvl)
        {
            case 1:
                nivelACargar = "01 Level 1";
                break;
            case 2:
                nivelACargar = "02 Level 2";
                break;
            case 3:
                nivelACargar = "03 Level 3";
                break;
            case 4:
                nivelACargar = "04 BossLevel 4";
                break;
        }
        StartCoroutine(LoadAsyncOperation());
   
        

    }


    IEnumerator LoadAsyncOperation()
    {

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nivelACargar);
        asyncOp.allowSceneActivation = false;
        while (asyncOp.isDone) { 
            yield return new WaitForEndOfFrame();
        }
        asyncOp.allowSceneActivation = true;
    }


}
