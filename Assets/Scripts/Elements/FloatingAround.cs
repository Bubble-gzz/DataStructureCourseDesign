using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAround : MonoBehaviour
{
    // Start is called before the first frame update
    bool visible, isFloating;
    [SerializeField]
    Vector2 startPos, endPos;
    SpriteRenderer sprite;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }
    void Start()
    {
        sprite.color = SetAlpha(sprite.color, 0);
        isFloating = false;
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (visible)
        {
            if (!isFloating)
            {
                StartCoroutine(_Floating());
            }
        }
    }
    IEnumerator _Floating()
    {
        isFloating = true;
        float progress = 0, speed = 3;
        while (progress < 1)
        {
            progress += speed * Time.deltaTime;
            transform.localPosition = Vector2.Lerp(startPos, endPos, Tween.EaseInOut(progress));
            if (!isFloating) yield break;
            yield return null;
        }
        progress = 0;
        while (progress < 1)
        {
            progress += speed * Time.deltaTime;
            transform.localPosition = Vector2.Lerp(endPos, startPos, Tween.EaseInOut(progress));
            if (!isFloating) yield break;
            yield return null;
        }
        isFloating = false;
    }
    public void Appear()
    {
        StartCoroutine(_Appear());
    }
    IEnumerator _Appear()
    {
        sprite.enabled = true;
        visible = true;
        transform.localPosition = startPos;
        float progress = sprite.color.a, speed = 4;
        while (progress < 1)
        {
            progress += speed * Time.deltaTime;
            sprite.color = SetAlpha(sprite.color, progress);
            if (!visible) yield break;
            yield return null;
        }
        sprite.color = SetAlpha(sprite.color, 1);
    }
    Color SetAlpha(Color color, float alpha)
    {
        Color newColor = color;
        newColor.a = alpha;
        return newColor;
    }
    public void Disappear()
    {
        StartCoroutine(_Disappear());
    }
    IEnumerator _Disappear()
    {
        visible = false;
        transform.localPosition = startPos;
        float progress = 1 - sprite.color.a, speed = 4;
        while (progress < 1)
        {
            progress += speed * Time.deltaTime;
            sprite.color = SetAlpha(sprite.color, 1 - progress);
            if (visible) yield break;
            yield return null;
        }
        sprite.color = SetAlpha(sprite.color, 0);
        isFloating = false;
        sprite.enabled = false;
    }
}
