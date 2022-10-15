using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class VisualizedSeqElement : MonoBehaviour
{
    [SerializeField]
    public List<Color> colors = new List<Color>();
    public enum ColorType{
        Normal,
        Pivot,
        Pointed
    }
    public string initialText;
    protected TMP_Text text;
    protected SpriteRenderer sprite;
    protected Canvas canvas;
    public AnimationBuffer animationBuffer;
    void Awake()
    {
        Transform child = transform.Find("Canvas/Text");
        text = child.GetComponent<TMP_Text>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
        animationBuffer = GetComponent<AnimationBuffer>();
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<ChangeColorAnimator>();
        gameObject.AddComponent<ChangeTextAnimator>();
        gameObject.AddComponent<PopAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();
        gameObject.AddComponent<WaitAnimator>();
    }
    public void SetText(string newText)
    {
        text.text = newText;
    }
}
