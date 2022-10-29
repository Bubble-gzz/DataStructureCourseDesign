using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Message : UIPanel
{
    // Start is called before the first frame update
    bool breath = false , breathing;
    TMP_Text text;
    override protected void Awake()
    {
        base.Awake();
        hovering = false;
        text = transform.Find("Panel/Text").GetComponent<TMP_Text>();
    }
    override protected void Update()
    {
        base.Update();
        if (breath && !breathing) StartCoroutine(_BreathOnce());
    }
    public void StartBreathing()
    {
        breath = true;
    }
    public void StopBreathing()
    {
        breath = false;
    }
    IEnumerator _BreathOnce()
    {
        breathing = true;
        //Debug.Log("Breath once");
        float progress = 0, speed = 0.8f;
        while (progress + speed * Time.deltaTime < 1)
        {
            progress += speed * Time.deltaTime;
            canvasGroup.alpha = 1 - Tween.SlowFastSlow(progress);
        //    Debug.Log("alpha = " + canvasGroup.alpha);
            if (fadingOut) yield break;
            yield return null;
        }
        progress = 1;
        canvasGroup.alpha = progress;
        yield return new WaitForSeconds(0.6f);
        breathing = false;
    }
    public void SetText(string newText)
    {
        text.text = newText;
    }
    public void Blink(bool destroyAfterBlink = true)
    {
        StartCoroutine(_Blink(destroyAfterBlink));
    }
    IEnumerator _Blink(bool destroyAfterBlink)
    {
        for (int i = 0; i < 5; i++)
        {
            canvasGroup.alpha = 0;
            yield return new WaitForSeconds(0.07f);
            canvasGroup.alpha = 1;
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.4f);
        if (destroyAfterBlink) FadeOut();
       // if (destroyAfterBlink) Destroy(gameObject);
    }
}
