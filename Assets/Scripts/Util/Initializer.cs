using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    AnimationBuffer animationBuffer;
    public static int frameID;
    void Start()
    {
        frameID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frameID = (frameID + 1) % 1000;
    }
}
