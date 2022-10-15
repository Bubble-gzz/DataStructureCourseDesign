using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAnimator : Animation
{
    Vector2 originScale;
    SpriteRenderer sprite;
    Canvas canvas;
    public PopAnimatorInfo info;
    delegate float EaseFunc(float x);
    public enum Type{
        Appear,
        Emphasize,
        Disappear
    }
    public Type type;
    protected override void Awake()
    {
        base.Awake();
        originScale = transform.localScale;
        sprite = GetComponent<SpriteRenderer>();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
    }
    private IEnumerator ChangeSize(float start, float end, float sec, EaseFunc easeFunc)
    {
        float size = start, timer = 0;
        transform.localScale = originScale * size;
        while (timer + Time.deltaTime < sec)
        {
            timer += Time.deltaTime;
            size = start + (end - start) * easeFunc(timer / sec);
            transform.localScale = originScale * size;
            yield return null;
        }
        size = end;
        transform.localScale = originScale * size;        
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
            sprite.enabled = true;
            canvas.enabled = true;
            yield return ChangeSize(0, 1.5f, 0.15f, Tween.EaseInOut);
            yield return ChangeSize(1.5f, 0.8f, 0.15f, Tween.EaseInOut);
            yield return ChangeSize(0.8f, 1.0f, 0.15f, Tween.EaseInOut);
        }
        else if (type == Type.Emphasize)
        {
            yield return ChangeSize(1.0f, 1.5f, 0.15f, Tween.EaseInOut);
            yield return ChangeSize(1.5f, 0.8f, 0.15f, Tween.EaseInOut);
            yield return ChangeSize(0.8f, 1.0f, 0.15f, Tween.EaseInOut);
        }
        else if (type == Type.Disappear)
        {
            yield return ChangeSize(1.0f, 1.15f, 0.15f, Tween.EaseInOut);
            yield return ChangeSize(1.15f, 0.0f, 0.15f, Tween.EaseInOut);
        }
        if (block) info.completed = true;
    }
}
