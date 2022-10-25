using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VisualizedNode : MonoBehaviour
{
    [SerializeField]
    public List<Color> colors = new List<Color>();
    public string initialText;
    protected TMP_Text text;
    protected SpriteRenderer sprite;
    protected Canvas canvas;
    public AnimationBuffer animationBuffer;
    public GraphNodeDragArea dragArea;
    Camera mainCam;
    public VisualizedGraph graph;
    public GraphNode node;
    Initializer initializer;
    void Awake()
    {
        Transform child = transform.Find("Canvas/Text");
        text = child.GetComponent<TMP_Text>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<ChangeColorAnimator>();
        gameObject.AddComponent<ChangeTextAnimator>();
        gameObject.AddComponent<PopAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();
        gameObject.AddComponent<WaitAnimator>();
    }
    void Start()
    {
        initializer = Global.initializer;
    }
    void Update()
    {
        if (node != null)
        {
            node.x = transform.localPosition.x;
            node.y = transform.localPosition.y;
        }
        sprite.enabled = true;
    }
    public void SetText(string newText)
    {
        text.text = newText;
        mainCam = Global.mainCamera;
        //transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    public void SetPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }
    public void DFS()
    {
        graph.DFS(node);
    }
    public void BFS()
    {
        graph.BFS(node);
    }
    public void Delete()
    {
        graph.DeleteNode(node);
    }
    public VisualizedEdgePro DragOutNewEdge()
    {
        return dragArea.DragOutNewEdge();
    }
}
