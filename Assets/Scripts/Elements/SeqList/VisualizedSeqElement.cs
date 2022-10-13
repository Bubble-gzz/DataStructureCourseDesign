using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class VisualizedSeqElement : MonoBehaviour
{
    // Start is called before the first frame update
    public float value;
    TMP_Text text;
    public AnimationBuffer animationBuffer;
    SpriteRenderer sprite;
    Canvas canvas;
    Vector2 originScale;
    [SerializeField]
    Color normalColor;
    [SerializeField]
    Color highlightColor;
    void Start()
    {
        Transform child = transform.Find("Canvas/Value");
        text = child.GetComponent<TMP_Text>();
        if (text) {
            Debug.Log("Find text");
        }
        else Debug.Log("Cannot find text");
        UpdateValue(value, false);
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
        originScale = transform.localScale;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator WaitForOrder()
    {
        int order;
        animationBuffer.latestOrder++;
        order = animationBuffer.latestOrder;
        while (animationBuffer.order != order) yield return null;
        animationBuffer.order++;
    }
    public void UpdatePos(Vector2 target, bool order = true)
    {
        StartCoroutine(_UpdatePos(target, order));
    }
    private IEnumerator _UpdatePos(Vector2 target, bool order)
    {
        if (order) {
            yield return WaitForOrder();
            while (true)
            {
                Vector2 delta = target - (Vector2)transform.position;
                float dist = delta.magnitude;
                if (dist < 0.01f) break;
                float speed = Mathf.Max(0.5f, dist * 10);
                transform.position += (Vector3)delta.normalized * speed * Time.deltaTime;
                yield return null;
            }
        }
        transform.position = target;
    }
    public void UpdateValue(float value, bool order = true)
    {
        StartCoroutine(_UpdateValue(value, order));
    }
    private IEnumerator _UpdateValue(float value, bool order)
    {
        if (order) yield return WaitForOrder();
        this.value = value;
        text.text = value.ToString("f0");
    }
    
    public void SelfDestroy(bool order = true)
    {
        StartCoroutine(_SelfDestroy(order));
    }
    private IEnumerator _SelfDestroy(bool order)
    {
        if (order) yield return WaitForOrder();
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
    delegate float EaseFunc(float x);
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
    public void PopOut(bool order = true)
    {
        StartCoroutine(_PopOut_Birth(order));
    }
    private IEnumerator _PopOut_Birth(bool order)
    {
        if (order) yield return WaitForOrder();
        sprite.enabled = true;
        canvas.enabled = true;
        yield return ChangeSize(0, 1.5f, 0.15f, Tween.EaseInOut);
        yield return ChangeSize(1.5f, 0.8f, 0.15f, Tween.EaseInOut);
        yield return ChangeSize(0.8f, 1.0f, 0.15f, Tween.EaseInOut);
    }
    private IEnumerator _PopOut_Emphasize()
    {
        yield return ChangeSize(1.0f, 1.5f, 0.15f, Tween.EaseInOut);
        yield return ChangeSize(1.5f, 0.8f, 0.15f, Tween.EaseInOut);
        yield return ChangeSize(0.8f, 1.0f, 0.15f, Tween.EaseInOut);
    }
    public void SetHighlight(bool flag, bool order = true)
    {
        StartCoroutine(_SetHighlight(flag, order));
    }
    private IEnumerator _SetHighlight(bool flag, bool order)
    {
        if (order) yield return WaitForOrder();
        float current = 0, speed = 5f;
        Color startColor = sprite.color, endColor;
        if (flag) {
            StartCoroutine(_PopOut_Emphasize());
            endColor = highlightColor;
        }
        else {
            endColor = normalColor;
        }
        while (1 - current > speed * Time.deltaTime)
        {
            current += speed * Time.deltaTime;
            sprite.color = Color.Lerp(startColor, endColor, current);
            yield return null;
        }
    }
}
