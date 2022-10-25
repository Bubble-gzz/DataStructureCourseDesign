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
        DFS,
        BFS
    }
    static public MouseMode mouseMode;
    static public bool mouseOverUI;
    static public Initializer initializer;
    static public bool pressEventConsumed;
    static public int debugCount;
}
