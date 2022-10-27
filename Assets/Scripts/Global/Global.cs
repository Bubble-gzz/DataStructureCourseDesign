using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    static public VisualizedGraph curGraph;
    static public bool loadGraphFromFiles;
    static public string filePath;
    static public string fileName;
    static public int waitingEventCount;
    static public void ChangeScene(string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
    }
}
