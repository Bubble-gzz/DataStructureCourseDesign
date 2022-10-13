using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    AnimationBuffer animationBuffer;
    void Start()
    {
        animationBuffer = GameObject.Find("AnimationBuffer").GetComponent<AnimationBuffer>();
        animationBuffer.order = 0;
        animationBuffer.latestOrder = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
