using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VisualizedGraph : MonoBehaviour
{
    // Start is called before the first frame update
    Graph graph;
    string graphName = "sampleGraph";
    [SerializeField]
    GameObject visualizedNodePrefab;
    [SerializeField]
    GameObject visualizedPointerPrefab;
    //int debugCount;
    AnimationBuffer animationBuffer;
    Camera mainCam;
    void Awake()
    {
        animationBuffer = GetComponent<AnimationBuffer>();
        gameObject.AddComponent<WaitAnimator>();
    }
    void NewGraph(int size = 100, bool directed = false)
    {
        graph = new Graph(size, directed); 
        graph.animationBuffer = animationBuffer;
        graph.image = gameObject;
        graph.pointer_cur = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        graph.pointer_cur.SetText("cur");
        graph.pointer_cur.offset = new Vector2(0,1);
    }
    void Start()
    {
        mainCam = Global.mainCamera;
        Global.mouseMode = Global.MouseMode.AddEdge;
        graphName = Global.fileName;
        if (!Global.loadGraphFromFiles) NewGraph(15, true);
        else LoadData(Global.filePath);
        Global.curGraph = this;
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            Global.mouseMode = Global.MouseMode.BFS;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(graph.ConvertToJsonData());
            SaveData();
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
    GraphNode NewNode(Vector2 pos, float value = 0)
    {
        GraphNode newNode = new GraphNode();
        VisualizedNode newVisualizedNode = Instantiate(visualizedNodePrefab, transform).GetComponent<VisualizedNode>();
        newNode.value = value;
        newNode.image = newVisualizedNode.gameObject;
        newNode.colors = newVisualizedNode.colors;
        newVisualizedNode.SetText(value.ToString("f0"));
        newVisualizedNode.SetPosition(pos);
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
    public bool AddEdge(VisualizedNode U, VisualizedNode V, GameObject edgeImage, ref Edge edgeInfo, float value = 0)
    {
        bool flag = graph.AddEdge(U.node.id, V.node.id, ref edgeInfo, edgeImage, value);
        if (!flag) Debug.Log("repetitive edge or self-loop!");
        return flag;
    }
    public void DeleteEdge(VisualizedNode U, VisualizedNode V)
    {
        graph.DeleteEdge(U.node.id, V.node.id);
    }
    public void DFS(GraphNode startNode)
    {
        graph.DFS(startNode);
    }
    public void BFS(GraphNode startNode)
    {
        graph.BFS(startNode);
    }
    public void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
    public void BuildFromJson(string jsonData)
    {
        Clear();
        GraphData data = JsonUtility.FromJson<GraphData>(jsonData);
        NewGraph(data.size, data.directed);
        graph.BuildFromJson(jsonData);
        for (int i = 0; i < data.size; i++)
            if (data.nodeRegistered[i])
            {
                GraphNode newNode = NewNode(data.pos[i], data.values[i]);
                Debug.Log("Created new node");
                graph.AddNode(newNode);
            }
        foreach (var edgeInfo in data.edges)
        {
            VisualizedNode U = graph.nodes[edgeInfo.u].image.GetComponent<VisualizedNode>();
            VisualizedNode V = graph.nodes[edgeInfo.v].image.GetComponent<VisualizedNode>();
            VisualizedEdgePro newEdge = U.DragOutNewEdge();
            Global.selectedNode = V.gameObject;
            newEdge.Draw(edgeInfo.value);
        }
    }
    public void SaveData()
    {
        string jsonData = graph.ConvertToJsonData();
        DirectoryInfo root = new DirectoryInfo(Application.dataPath + "/GraphData/");
        if (!root.Exists) root.Create();
        string path = root + graphName + ".data";
        File.WriteAllText(path, jsonData);
    }
    public void LoadData(string path)
    {
        string jsonData = File.ReadAllText(path);
        BuildFromJson(jsonData);
    }
}
