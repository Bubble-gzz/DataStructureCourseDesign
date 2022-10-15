using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class VisualizedPointer : MonoBehaviour
{
    Canvas canvas;
    void Start()
    {
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
    }
}
