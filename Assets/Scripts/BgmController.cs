using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmController : MonoBehaviour
{
    public Slider backVolume;
    public AudioSource audio;
    private float backVol = 1f;

    void Start()
    {
        backVolume = GameObject.Find("music_Slider").GetComponent<Slider>();
        audio = GetComponent<AudioSource>();

        backVol = PlayerPrefs.GetFloat("backvol",1f);
        backVolume.value = backVol;
        audio.volume = backVolume.value;
    }

    void Update()
    {
        SoundSlider();
    }

    public void SoundSlider()
    {
        audio.volume = backVolume.value;

        backVol = backVolume.value;
        PlayerPrefs.SetFloat("backvol", backVol);
    }
}
