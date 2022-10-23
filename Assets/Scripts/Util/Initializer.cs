using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    AnimationBuffer animationBuffer;
    public static int frameID;
    int count;
   // [SerializeField]
   // public float animationTimeScale = 1.0f;
    void Awake()
    {
        Global.initializer = this;
    }
    void Start()
    {
        frameID = 0;
        Global.mouseOverUI = false;
        count = 0;
    }

    void Update()
    {
        frameID = (frameID + 1) % 1000;
        //Settings.animationTimeScale = this.animationTimeScale;
    }
}
