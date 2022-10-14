using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRoot : MonoBehaviour
{
    // Start is called before the first frame update
    Node child;
    bool isDragged;
    Vector2 mouseLastPos;
    public bool mouseHovering;
    void Start()
    {
        child = transform.Find("body").GetComponent<Node>();
        isDragged = false;
        mouseHovering = false;
    }

    void Update()
    {
        isDragged = DragCheck();
    }
    void OnMouseOver()
    {
        if (!mouseHovering) {
            mouseHovering = true;
            MouseManager.hoverCount++;
            child.OnMouseEnter();
        }
    }
    void OnMouseExit()
    {
        mouseHovering = false;
        MouseManager.hoverCount--;
        child.OnMouseExit();
    }
    bool DragCheck()
    {
        if (!mouseHovering) return false;
        if (MouseManager.editMode != MouseManager.EditMode.Create) return false;
        if (!Input.GetMouseButton(0)) return false;
        Vector2 pos = Global.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (isDragged) {
            transform.position += (Vector3)(pos - mouseLastPos);
        }
        mouseLastPos = pos;
        return true;
    }
}
