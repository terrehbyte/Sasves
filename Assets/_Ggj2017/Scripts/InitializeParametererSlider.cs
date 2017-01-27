using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InitializeParametererSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer targetMixer;

    [SerializeField]
    private string parameterName;

    void Start()
    {
        float temp;
        targetMixer.GetFloat(parameterName, out temp);

        GetComponent<Slider>().value = temp;
    }
}
