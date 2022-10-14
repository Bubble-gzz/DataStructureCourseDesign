using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class VisualizedElement : MonoBehaviour
{
    // Start is called before the first frame update
    protected TMP_Text text;
    public AnimationBuffer animationBuffer;
    protected SpriteRenderer sprite;
    protected Canvas canvas;
    Vector2 originScale;
    class AnimationQueue{
        int capacity;
        int latestOrder;
        int currentOrder;
        public AnimationQueue(int capacity = 1000)
        {
            this.capacity = capacity;
            this.latestOrder = -1;
            this.currentOrder = 0;
        }
        public int NewOrder()
        {
            latestOrder = (latestOrder + 1) % capacity;
            return latestOrder;
        }
        public void FinishOrder()
        {
            currentOrder = (currentOrder + 1) % capacity;
        }
        public bool isLatest(int order)
        {
            return order == latestOrder;
        }
    }
    AnimationQueue updatePosQueue = new AnimationQueue(10);
    virtual protected void Start()
    {
        originScale = transform.localScale;
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
            int myOrder = updatePosQueue.NewOrder();
            while (true)
            {
                Vector2 delta = target - (Vector2)transform.position;
                float dist = delta.magnitude;
                if (dist < 0.01f) break;
                float speed = Mathf.Max(0.5f, dist * 10);
                transform.position += (Vector3)delta.normalized * speed * Time.deltaTime;
                yield return null;
                if (!updatePosQueue.isLatest(myOrder)) {
                    yield break;
                }
            }
        }
        transform.position = target;
    }

    public void UpdateText(string newText, bool order = true)
    {
        StartCoroutine(_UpdateText(newText, order));
    }
    private IEnumerator _UpdateText(string newText, bool order)
    {
        if (order) {
            yield return WaitForOrder();
             StartCoroutine(_PopOut_Emphasize());
            text.text = newText;
        }
        else 
        {
            text.text = newText;
        }
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
    public void PopOut_Birth(bool order = true)
    {
        StartCoroutine(_PopOut_Birth(order));
    }
    public void PopOut_Emphasize()
    {
        StartCoroutine(_PopOut_Emphasize());
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
        yield return WaitForOrder();
        yield return ChangeSize(1.0f, 1.5f, 0.15f, Tween.EaseInOut);
        yield return ChangeSize(1.5f, 0.8f, 0.15f, Tween.EaseInOut);
        yield return ChangeSize(0.8f, 1.0f, 0.15f, Tween.EaseInOut);
    }
    public void SetColor(Color targetColor, bool order = true)
    {
        StartCoroutine(_SetColor(targetColor, order));
    }
    private IEnumerator _SetColor(Color targetColor, bool order)
    {
        if (order) yield return WaitForOrder();
        float current = 0, speed = 5f;
        Color startColor = sprite.color, endColor = targetColor;
        while (1 - current > speed * Time.deltaTime)
        {
            current += speed * Time.deltaTime;
            sprite.color = Color.Lerp(startColor, endColor, current);
            yield return null;
        }
    }
}
