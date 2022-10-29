using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VisualizedNode : VisualizedElement
{
    [SerializeField]
    public string initialText;
    public GraphNodeDragArea dragArea;
    public VisualizedGraph graph;
    Initializer initializer;
    override protected void Awake()
    {
        base.Awake();
        appearOnCreate = false;
        interactable = false;
    }
    override protected void Start()
    {
        initializer = Global.initializer;
    }
    void Update()
    {
        if (info != null)
        {
            info.x = transform.localPosition.x;
            info.y = transform.localPosition.y;
        }
        sprite.enabled = true;
    }
    public void SetPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }
    public void DFS()
    {
        graph.DFS((GraphNode)info);
    }
    public void BFS()
    {
        graph.BFS((GraphNode)info);
    }
    public void Dijkstra()
    {
        graph.Dijkstra((GraphNode)info);
    }
    override public void OnDelete()
    {
        graph.DeleteNode((GraphNode)info);
    }
    public VisualizedEdgePro DragOutNewEdge()
    {
        return dragArea.DragOutNewEdge();
    }
}
