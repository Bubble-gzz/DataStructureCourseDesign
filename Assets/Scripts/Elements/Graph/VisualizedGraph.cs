using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizedGraph : MonoBehaviour
{
    // Start is called before the first frame update
    Graph graph;
    [SerializeField]
    GameObject visualizedNodePrefab;
    [SerializeField]
    GameObject visualizedPointerPrefab;
    int debugCount;
    AnimationBuffer animationBuffer;
    Camera mainCam;
    void Awake()
    {
        graph = new Graph(); 
        animationBuffer = GetComponent<AnimationBuffer>();
        graph.animationBuffer = animationBuffer;
        graph.image = gameObject;
        gameObject.AddComponent<WaitAnimator>();

        graph.pointer_cur = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        graph.pointer_cur.SetText("cur");
        graph.pointer_cur.offset = new Vector2(0,1);
        
        debugCount = 0;
    }
    void Start()
    {
        mainCam = Global.mainCamera;
        Global.mouseMode = Global.MouseMode.AddEdge;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddNode();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Global.mouseMode == Global.MouseMode.Move)
                Global.mouseMode = Global.MouseMode.AddEdge;
            else Global.mouseMode = Global.MouseMode.Move;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Global.mouseMode = Global.MouseMode.DFS;
        }
    }

    GraphNode NewNode(float value = 0)
    {
        GraphNode newNode = new GraphNode();
        VisualizedNode newVisualizedNode = Instantiate(visualizedNodePrefab, transform).GetComponent<VisualizedNode>();
        newNode.value = value;
        newNode.image = newVisualizedNode.gameObject;
        newNode.colors = newVisualizedNode.colors;
        newVisualizedNode.SetText(value.ToString("f0"));
        newVisualizedNode.SetPosition(mainCam.ScreenToWorldPoint(Input.mousePosition));
        newVisualizedNode.graph = this;
        newVisualizedNode.node = newNode;
        return newNode;
    }
    public void AddNode()
    {
        GraphNode newNode = NewNode();
        graph.AddNode(newNode);
    }
    public void DeleteNode(GraphNode node)
    {
        graph.DeleteNode(node);
    }
    public bool AddEdge(VisualizedNode U, VisualizedNode V, GameObject edgeImage)
    {
        bool flag = graph.AddEdge(U.node.id, V.node.id, edgeImage);
        if (!flag) Debug.Log("repetitive edge or self-loop!");
        return flag;
    }
    public void DFS(GraphNode startNode)
    {
        graph.DFS(startNode);
    }
}
