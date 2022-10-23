using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EdgePanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject edge;
    [SerializeField]
    UIPanel panel;
    public void Delete()
    {
        edge.GetComponentInChildren<VisualizedEdgePro>().Delete();
        Global.mouseOverUI = false;
        panel.FadeOut();
    }
    public void OnValueChanged(string newText)
    {
        float newValue;
        try{
            if (newText == "") newValue = 0;
            else newValue = float.Parse(newText);
            edge.GetComponent<VisualizedEdgePro>().info.UpdateValue(newValue);
        }
        catch (Exception e)
        {

        }
    }
}
