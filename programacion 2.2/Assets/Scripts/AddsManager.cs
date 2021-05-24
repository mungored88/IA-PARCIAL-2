using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AddsManager : MonoBehaviour
{
    public enum TypeAds
    {
        video,
        rewardedVideo
    };
    public TypeAds currentType;

    string _gameId = "3913047";

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(_gameId, false);
    }

  
    public void Show() 
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(currentType.ToString(), new ShowOptions() {resultCallback = ResulAds});
        }
    }
    void ResulAds(ShowResult result)
    {
        if (result==ShowResult.Failed)
        {
            //no da nada
            SceneManager.LoadScene("00 menu");
        }
        else if(result==ShowResult.Skipped)
        {
            //si lo skipeo
            SceneManager.LoadScene("00 menu");
        } else
        {
            int lvl = FindObjectOfType<ObjParaCargarAsync>().nivelACargar;
            string scene = "0"+lvl+" level " + lvl;
            if (lvl == 4) scene = "04 BossLevel 4";
            SceneManager.LoadScene(scene);

        }
    }
   

}
