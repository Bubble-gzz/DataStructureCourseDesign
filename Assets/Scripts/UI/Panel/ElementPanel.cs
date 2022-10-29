using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ElementPanel : UIPanel
{
    public VisualizedElement element;
    override protected void Awake()
    {
        base.Awake();
    }
    public void OnValueChanged(string newText)
    {
        float newValue; Debug.Log("newText : " + newText);
        try{
            if (newText == "") newValue = 0;
            else newValue = float.Parse(newText);
            element.info.UpdateValue(newValue);
        }
        catch (Exception e)
        {
            Debug.Log("Invalid Input Error : " + e);
        }
    }
    virtual public void OnDelete()
    {
        element.OnDelete();
    }
}
