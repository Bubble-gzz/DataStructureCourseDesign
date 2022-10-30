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
    FloatingAround triangleU, triangleD;
    VisualizedSeqElement root;
    bool freezeExitCheck;
    override protected void Awake()
    {
        base.Awake();
        if (type == Type.InsertButton)
        {
            triangleU = transform.Find("Triangle_U").GetComponent<FloatingAround>();
            triangleD = transform.Find("Triangle_D").GetComponent<FloatingAround>();
            freezeExitCheck = false;
        }
    }
    override protected void Start()
    {
        base.Start();
        if (type == Type.Normal)
        {
            VisualizedSeqElement insertButton = transform.Find("InsertButtonElement").GetComponent<VisualizedSeqElement>();
            insertButton.root = this;
            insertButton.list = list;
        }
    }
    override public void OnDelete()
    {
        list.Delete(( (SeqElement)info ).pos, true);
    }
    override protected void OnInsertButtonEnter()
    {
        if (list.freezeInsertButton) return;
        triangleU.Appear();
        triangleD.Appear();
        root.interval = 0.6f;
        myCollider.transform.localScale = new Vector2(0.7f, 1.2f);
        transform.localPosition = new Vector2(-0.9f, 0);
        list.RefreshPos();
        canvas.enabled = true;
    }
    override protected void OnInsertButtonExit()
    {
        triangleU.Disappear();
        triangleD.Disappear();
        if (!list.freezeInsertButton) root.interval = root._interval;
        myCollider.transform.localScale = new Vector2(0.3f, 1.2f);
        transform.localPosition = new Vector2(-0.8f, 0);
        if (!freezeExitCheck) list.RefreshPos();
        canvas.enabled = false;
    }
    override protected void MyOnMouseClick()
    {
        if (!alive) return;
        if (root != null && !root.alive) return;
        if (type == Type.Ghost) return;
        if (type == Type.InsertButton) {
            Debug.Log("root.info : " + root.info + "   list : " + list);
            root.interval = 0.3f;
            StartCoroutine(_FreezeExitCheck());
            list.Insert(((SeqElement)(root.info)).pos);
            return ;
        }
        if (type == Type.AppendButton) {
            list.Append();
            return;
        }
        GameObject newPanel = Instantiate(panelPrefab);
        newPanel.GetComponentInChildren<ElementPanel>().element = this;
        newPanel.transform.position = transform.position + panelOffset;
    }
    IEnumerator _FreezeExitCheck()
    {
        list.freezeInsertButton = true;
        yield return new WaitForSeconds(0.7f);
        list.freezeInsertButton = false;
    }
}
