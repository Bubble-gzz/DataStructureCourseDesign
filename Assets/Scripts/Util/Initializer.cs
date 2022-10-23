using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    AnimationBuffer animationBuffer;
    public static int frameID;
    bool pressCoolingDown;
    //int count;
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
        pressCoolingDown = false;
    }

    void Update()
    {
        frameID = (frameID + 1) % 1000;
        Global.pressEventConsumed = false;
        //if (Global.pressEventConsumed && !pressCoolingDown)
        //    StartCoroutine(PressCoolDown());
        //Debug.Log("[frame:"+ Time.frameCount +"]");
        //if (Input.anyKeyDown) Debug.Log("[frame:"+ Time.frameCount +"] anyKeyDown = true");
    }
    IEnumerator PressCoolDown()
    {
        pressCoolingDown = true;
        while (Input.anyKey) yield return null;
        Global.pressEventConsumed = false;
        pressCoolingDown = false;
    }
}
