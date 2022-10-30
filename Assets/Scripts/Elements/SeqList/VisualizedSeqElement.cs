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
    public VisualizedSeqList list;
    override protected void Awake()
    {
        base.Awake();
        interactable = true;
    }
    override public void OnDelete()
    {
        list.Delete(( (SeqElement)info ).pos, true);
    }
    override protected void MyOnMouseClick()
    {
        if (!alive) return;
        if (type == Type.Ghost) return;
        if (type == Type.AppendButton) {
            list.Append();
            return;
        }
        GameObject newPanel = Instantiate(panelPrefab);
        newPanel.GetComponentInChildren<ElementPanel>().element = this;
        newPanel.transform.position = transform.position + panelOffset;
    }
}
