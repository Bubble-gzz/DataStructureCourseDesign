using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    // Start is called before the first frame update
    MyCollider clickArea;
    AnimationBuffer animationBuffer;
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
        clickArea = transform.Find("ClickArea").GetComponent<MyCollider>();
        clickArea.onMouseClick.AddListener(MyOnMouseClick);
        clickArea.onMouseEnter.AddListener(MyOnMouseEnter);
        clickArea.onMouseExit.AddListener(MyOnMouseExit);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MyOnMouseClick()
    {
        
    }
    void MyOnMouseEnter()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut));
    }
    void MyOnMouseExit()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack));
    }
}
