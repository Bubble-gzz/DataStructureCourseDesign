using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualizedStackElement : VisualizedElement
{
    public enum ColorType{
        Normal,
        Pivot,
        Pointed
    }
    public VisualizedLinkStack stack;
    override protected void Awake()
    {
        base.Awake();
        panelOffset = new Vector3(2, 0, -1);
    }
}
