using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolumen : MonoBehaviour
{
    public AudioMixer mixer;
    public string Fader;
    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat(Fader, Mathf.Log10(sliderValue) * 20);
    }
}
