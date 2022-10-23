using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AnimationSpeedSlider : MonoBehaviour
{
    // Start is called before the first frame update
    Slider slider;
    TMP_Text value;
    void Awake()
    {
        slider = GetComponent<Slider>();
        value = transform.Find("value").GetComponent<TMP_Text>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < 0.01f) {
            Settings.animationTimeScale = -1;
            value.text = "PRESS";
        }
        else if (slider.value > slider.maxValue - 0.01f) {
            Settings.animationTimeScale = 0;
            value.text = "MAX";
        }
        else {
            Settings.animationTimeScale = 1.0f / slider.value;
            value.text = slider.value.ToString("f2");
        }
    }
}
