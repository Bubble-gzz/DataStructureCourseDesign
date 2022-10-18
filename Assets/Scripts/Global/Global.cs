using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
    // Start is called before the first frame update
    static public Camera mainCamera;
    static public GameObject selectedNode;
    public enum MouseMode{
        AddEdge,
        Move,
        DFS
    }
    static public MouseMode mouseMode;
}
