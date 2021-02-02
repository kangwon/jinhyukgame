using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundController : MonoBehaviour
{
    public static WorldSoundController WSC_instance;
    public GameObject W1_bgm;
    public GameObject W2_bgm;
    public int SoundState = 0;

    private void Awake()
    {
        WSC_instance = this;
    }

    public void WorldSoundControl()
    {
        AudioSource W1Sound = W1_bgm.GetComponent<AudioSource>();
        AudioSource W2Sound = W2_bgm.GetComponent<AudioSource>();
        if (SoundState == 0 )
        {
            W1Sound.enabled = false;
            W2Sound.enabled = false;
        }
        else if (SoundState == 1)
        {
            W1Sound.enabled = true;
            W2Sound.enabled = false;
        }
        else if (SoundState == 2)
        {
            W1Sound.enabled = false;
            W2Sound.enabled = true;
        }
        else
        {
            W1Sound.enabled = false;
            W2Sound.enabled = false;
        }
    }
}