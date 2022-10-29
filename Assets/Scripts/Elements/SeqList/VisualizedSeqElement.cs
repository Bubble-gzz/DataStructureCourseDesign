using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class VisualizedSeqElement : VisualizedElement
{
    public enum ColorType{
        Normal,
        Pivot,
        Pointed
    }
    public SeqElement info;
    public VisualizedSeqList list;
    override protected void Awake()
    {
        base.Awake();
        interactable = true;
    }
    override public void OnDelete()
    {
        list.Delete(info.pos, true);
    }
}
