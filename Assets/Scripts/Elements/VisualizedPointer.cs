using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class VisualizedPointer : VisualizedElement
{
    protected override void Start()
    {
        base.Start();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
    }
    // Start is called before the first frame update
}
