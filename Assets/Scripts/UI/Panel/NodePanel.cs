using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePanel : MonoBehaviour
{
    public GameObject node;
    [SerializeField]
    UIPanel panel;
    public void Delete()
    {
        node.GetComponentInChildren<VisualizedNode>().Delete();
        Global.mouseOverUI = false;
        panel.FadeOut();
    }
}
