using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class VisualizedSeqElement : VisualizedElement
{
    [SerializeField]
    public List<Color> colors = new List<Color>();
    public enum ColorType{
        Normal,
        Pivot,
        Pointed
    }
    protected override void Start()
    {
        base.Start();
        Transform child = transform.Find("Canvas/Value");
        text = child.GetComponent<TMP_Text>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
    }
}
