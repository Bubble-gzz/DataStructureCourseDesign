using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class VisualizedPointer : MonoBehaviour
{
    public Canvas canvas;
    public Vector2 offset;
    AnimationBuffer animationBuffer;
    protected TMP_Text text;
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        text = transform.Find("Canvas/Text").GetComponent<TMP_Text>();
        canvas.enabled = false;
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<PopAnimator>();
    }
    public Vector2 CalcPos(Vector2 pos)
    {
        return pos + offset;
    }
    public void SetText(string newText)
    {
        //Debug.Log(newText);
        text.text = newText;
    }
}
