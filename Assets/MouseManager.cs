using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseManager:MonoBehaviour
{
    static public int zoneLevel;
    public enum EditMode{
        Create,
        Edit
    }

    static public EditMode editMode;
    static public int hoverCount;
    protected virtual void Start()
    {
        zoneLevel = 0;
        hoverCount = 0;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
    }
    protected virtual void LeftClick()
    {

    }
}
