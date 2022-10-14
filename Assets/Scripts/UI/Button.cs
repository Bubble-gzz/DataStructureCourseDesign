using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int zoneLevel = 1;
    Vector2 scale0;
    bool hovering;
    void Start()
    {
        scale0 = transform.localScale;
        hovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (!hovering) {
            hovering = true;
            MouseManager.hoverCount++;
        }
        transform.localScale = scale0 * 1.2f;
    }
    void OnMouseExit()
    {
        hovering = false;
        MouseManager.hoverCount--;
        transform.localScale = scale0;
    }
}
