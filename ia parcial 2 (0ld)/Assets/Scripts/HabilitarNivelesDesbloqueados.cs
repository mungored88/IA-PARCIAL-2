using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilitarNivelesDesbloqueados : MonoBehaviour
{
    public SaveLoad saveload;

    public GameObject[] listaDeBotones;

    void Start() {
        DataToSave savedGame = saveload.LoadFile();
        for (int i = 0; i < savedGame.nivelesDesbloqueados; i++)
        {
            listaDeBotones[i].SetActive(true);
        }
    } 

    public void hackearLevelsForTest() {
        for (int i = 0; i < listaDeBotones.Length; i++)
        {
            listaDeBotones[i].SetActive(true);
        }
    }
}
