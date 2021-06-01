using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource Audio;

    public AudioClip ButtonClick;
    public AudioClip Win;
    public AudioClip Lose;
    public AudioClip Kill;
    public AudioClip Move;

    public static SFXManager SFXInstance;

    private void Awake()
    {
        if (SFXInstance != null && SFXInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        SFXInstance = this;
        DontDestroyOnLoad(this);

    }

    public void playSFX(AudioClip audio)
    {
        SFXManager.SFXInstance.Audio.volume = 1.0f;
        SFXManager.SFXInstance.Audio.PlayOneShot(audio);
    }

    public void playMove()
    {
        SFXManager.SFXInstance.Audio.volume = 1.0f;
        SFXManager.SFXInstance.Audio.PlayOneShot(SFXManager.SFXInstance.Move);
    }
}
