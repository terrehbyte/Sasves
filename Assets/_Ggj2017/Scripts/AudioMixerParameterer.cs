using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioMixerParameterer : MonoBehaviour
{
    public AudioMixer targetMixer;

    //[System.Serializable]
    //public class SliderParameterPair
    //{
    //    public Slider slider;
    //    public string parameter;

    //    private UnityAction<float> method;

    //    public void Link()
    //    {
    //        method += v => ;
    //    }

    //    ~SliderParameterPair()
    //    {
    //        if(method != null)
    //        {
    //            slider.onValueChanged.RemoveListener(method)
    //        }
    //    }
    //}
    //[SerializeField]
    //private SliderParameterPair[] pairs;

    //private void Start()
    //{
    //    for(int i = 0; i < pairs.Length; ++i)
    //    {
    //        pairs[i].slider.onValueChanged += {  }
    //    }
    //}

    //private void OnDestroy()
    //{
    //    for (int i = 0; i < pairs.Length; ++i)
    //    {
    //        pairs[i].slider.onValueChanged += { }
    //    }
    //}

    public void SetMasterVol(float volume)
    {
        targetMixer.SetFloat("masterLevel", volume);
    }

    public void SetMusicVol(float volume)
    {
        targetMixer.SetFloat("musicLevel", volume);
    }

    public void SetSoundVol(float volume)
    {
        targetMixer.SetFloat("soundLevel", volume);
    }
}
