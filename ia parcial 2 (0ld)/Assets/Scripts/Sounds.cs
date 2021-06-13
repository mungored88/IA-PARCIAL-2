using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;
    public void SoundPlay(AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }
    public void SoundPlayRandom()
    {
        var random = Random.Range(0, clips.Length - 1);
        SoundPlay(clips[random]);
    }
}
