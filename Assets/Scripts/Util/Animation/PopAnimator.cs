using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAnimator : Animation
{
    Vector2 originScale;
    Canvas canvas;
    public PopAnimatorInfo info;
    SpriteRenderer sprite;
    delegate float EaseFunc(float x);
    public enum Type{
        Appear,
        Emphasize,
        Disappear,
        PopOut,
        PopBack,
        AppearLine,
        DisappearLine,
        PopOutLine,
        PopBackLine,
        EmphasizeLine
    }
    public Type type;
    protected override void Awake()
    {
        base.Awake();
        originScale = transform.localScale;
        sprite = GetComponentInChildren<SpriteRenderer>();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
    }
    private IEnumerator ChangeSize(float start, float end, float sec, int myOrder, EaseFunc easeFunc)
    {
        float size, timer = 0;
        if (start < 0) start = transform.localScale.x / originScale.x; // continue with current size
        size = start;
        //Debug.Log("localScale = " + transform.localScale);
        transform.localScale = originScale * size;
        while (timer + Time.deltaTime < sec)
        {
            timer += Time.deltaTime;
            size = start + (end - start) * easeFunc(timer / sec);
            transform.localScale = originScale * size;
            if (!animationOrder.isLatest(myOrder)) yield break;
            yield return null;
        }
        size = end;
        transform.localScale = originScale * size;        
    }
    private IEnumerator ChangeWidth(float start, float end, float sec, int myOrder, EaseFunc easeFunc)
    {
        float size, timer = 0;
        originScale.x = transform.localScale.x;
        if (start < 0) start = transform.localScale.x / originScale.x; // continue with current size
        size = start;
        //Debug.Log("localScale = " + transform.localScale);
        transform.localScale = new Vector2(originScale.x, originScale.y * size);
        while (timer + Time.deltaTime < sec)
        {
            timer += Time.deltaTime;
            size = start + (end - start) * easeFunc(timer / sec);
            transform.localScale = new Vector2(originScale.x, originScale.y * size);
            if (!animationOrder.isLatest(myOrder)) yield break;
            yield return null;
        }
        size = end;
        transform.localScale = new Vector2(originScale.x, originScale.y * size);        
    }
    protected override IEnumerator Animate()
    {
        Type type = this.type;
        PopAnimatorInfo info = this.info;
        bool block = this.block;
        int myOrder = animationOrder.NewOrder();
        if (!block) info.completed = true;
        
        if (type == Type.Appear)
        {
            if (sprite != null) sprite.enabled = true;
            if (canvas != null) canvas.enabled = true;
            yield return ChangeSize(0, 1.5f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.5f, 0.8f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(0.8f, 1.0f, 0.15f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.Emphasize)
        {
            yield return ChangeSize(1.0f, 1.5f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.5f, 0.8f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(0.8f, 1.0f, 0.15f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.Disappear)
        {
            yield return ChangeSize(1.0f, 1.15f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.15f, 0.0f, 0.15f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopOut)
        {
            yield return ChangeSize(-1, 1.4f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.4f, 1.2f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.2f, 1.25f, 0.1f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopBack)
        {
            yield return ChangeSize(-1, 0.9f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(0.9f, 1.15f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.15f, 1.0f, 0.1f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.AppearLine)
        {
            if (sprite != null) sprite.enabled = true;
            if (canvas != null) canvas.enabled = true;
            yield return ChangeWidth(0, 1.5f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(1.5f, 0.8f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(0.8f, 1.0f, 0.15f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.DisappearLine)
        {
            yield return ChangeWidth(1.0f, 1.15f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(1.15f, 0.0f, 0.15f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopOutLine)
        {
            yield return ChangeWidth(-1, 1.4f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(1.4f, 1.2f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(1.2f, 1.25f, 0.1f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopBackLine)
        {
            yield return ChangeWidth(-1, 0.9f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(0.9f, 1.15f, 0.1f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(1.15f, 1.0f, 0.1f, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.EmphasizeLine)
        {
            yield return ChangeWidth(1.0f, 1.5f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(1.5f, 0.8f, 0.15f, myOrder, Tween.EaseInOut);
            yield return ChangeWidth(0.8f, 1.0f, 0.15f, myOrder, Tween.EaseInOut);
        }
        if (block) info.completed = true;
    }
}
