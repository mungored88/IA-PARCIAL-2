using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerView : MonoBehaviour, IObserver
{
    public GameObject shield;
    public AudioClip hurtSound;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI shieldText;

    public TextMeshProUGUI magnetText;
    public TextMeshProUGUI speedText;
    public delegate void Execute();

    private Animator _anim;
    private AudioSource _audioSource;
    private int _shieldHits;
    private Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    void Start()
    {
        shield.SetActive(false);
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        _anim.SetTrigger("Jump");
    }

    public void Move(float x, float y)
    {
        _anim.SetFloat("SpeedX", x);
        _anim.SetFloat("SpeedY", y);
    }

    public void HealthUpdate(float healthPoints)
    {
        lifeText.text = string.Format("Life: {0}", healthPoints);
    }

    public void UpdateSpeedText(float time)
    {
        if (!speedText.gameObject.activeSelf)
            speedText.gameObject.SetActive(true);
        speedText.text = string.Format("Speed: {0}sec.", time);
    }

    public void DisableSpeedText()
    {
        speedText.gameObject.SetActive(false);
    }

    public void UpdateShieldText(int shieldAmount)
    {
        if (!shieldText.gameObject.activeSelf)
            shieldText.gameObject.SetActive(true);
        shieldText.text = string.Format("Shields: {0}", shieldAmount);
        shield.SetActive(true);
    }

    public void ShieldEffectOff()
    {
        shieldText.gameObject.SetActive(false);
        shield.SetActive(false);
    }

    public void UpdateMagnetText(float time)
    {
        if (!magnetText.gameObject.activeSelf)
            magnetText.gameObject.SetActive(true);
        magnetText.text = string.Format("Magnet: {0} sec.", time);
    }

    public void MagnetEffectOff()
    {
        magnetText.gameObject.SetActive(false);
    }

    public void Hurt()
    {
        _anim.SetTrigger("Hitted");
        _audioSource.clip = hurtSound;
        _audioSource.Play();
    }

    public void OnAction(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
    }
}
