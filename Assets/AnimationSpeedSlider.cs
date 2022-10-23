using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AnimationSpeedSlider : UIPanel
{
    // Start is called before the first frame update
    Slider slider;
    TMP_Text value;
    [SerializeField]
    float minSpeed = 0.5f, maxSpeed = 8f;
    override protected void Awake()
    {
        base.Awake();
        slider = GetComponent<Slider>();
        value = transform.Find("value").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    override protected void Update()
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
            Settings.animationTimeScale = 1.0f / (minSpeed + (slider.value / slider.maxValue) * (maxSpeed - minSpeed));
            //Debug.Log("Settings.animationTimeScale" + Settings.animationTimeScale);
            value.text = (slider.value / slider.maxValue * 100).ToString("f0") + "%";
        }
    }
}
