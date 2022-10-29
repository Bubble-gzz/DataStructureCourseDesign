using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualizedPointer : MonoBehaviour
{
    public Canvas canvas;
    public Vector2 offset;
    public AnimationBuffer animationBuffer;
    protected TMP_Text text;
    void Awake()
    {
        //animationBuffer = gameObject.AddComponent<AnimationBuffer>();
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

    public void Appear(bool block = false)
    {
        PopAnimatorInfo info = new PopAnimatorInfo(gameObject, PopAnimator.Type.Appear);
        info.block = block;
        animationBuffer.Add(info);
    }
    public void ChangePos(Vector2 newPos, bool animated = true, bool local = false)
    {
        UpdatePosAnimatorInfo info = new UpdatePosAnimatorInfo(gameObject, newPos + offset, animated, local);
        info.block = true;
        animationBuffer.Add(info);
    }
    public void Disappear(bool block = false)
    {
        PopAnimatorInfo info = new PopAnimatorInfo(gameObject, PopAnimator.Type.Disappear);
        info.block = block;
        animationBuffer.Add(info);
    }
}
