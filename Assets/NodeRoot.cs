using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRoot : MonoBehaviour
{
    // Start is called before the first frame update
    Node child;
    void Start()
    {
        child = transform.Find("body").GetComponent<Node>();
    }

    // Update is called once per frame
    void OnMouseEnter()
    {
        child.OnMouseEnter();
    }
    void OnMouseExit()
    {
        child.OnMouseExit();
    }
}
