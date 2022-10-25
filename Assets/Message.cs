using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Message : UIPanel
{
    // Start is called before the first frame update
    bool breath, breathing;
    TMP_Text text;
    override protected void Awake()
    {
        base.Awake();
        hovering = false;
        StartBreathing();
        text = transform.Find("Panel/Text").GetComponent<TMP_Text>();
    }
    override protected void Update()
    {
        base.Update();
        if (breath && !breathing) StartCoroutine(_BreathOnce());
    }
    void StartBreathing()
    {
        breath = true;
    }
    void StopBreathing()
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
}
