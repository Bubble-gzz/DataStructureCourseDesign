using System;
using System.IO;
using System.Runtime.InteropServices;
using DataStructureCourseDesign.SeqListSpace;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class GraphNode : DataElement
    {
        public string name;

        public Graph graph;
        public Edge firstEdge;
        public Edge backTrace;
        public int indgr;
        
        public bool flag;

        public bool visited {
            set { flag = value;  }
            get { return flag;  }
        }
        public float minDist {
            set { this.value = value; }
            get { return this.value; }
        }
        
        public float minWeight {
            set { this.value = value; }
            get { return this.value; }
        }

        public GraphNode(string _name = "")
        {
            name = _name;
            x = y = 0;
            firstEdge = null;
        }
        public void UpdateName(string newName)
        {
            name = newName;
            if (image == null) return ;
            animationBuffer.Add(new ChangeTextAnimatorInfo(image, newName));
        }
        public void UpdateRank(int newRank)
        {
            if (image == null) return ;
            string newText;
            if (newRank == 1) newText = "1st";
            else if (newRank == 2) newText = "2nd";
            else if (newRank == 3) newText = "3rd";
            else newText = newRank.ToString() + "th";
            animationBuffer.Add(new ChangeTextAnimatorInfo(image, newText));            
        }
    }
    public class Edge : DataElement
    {
        public GraphNode startNode, endNode;
        public Graph graph;
        public float length {
            set { this.value = value; }
            get { return this.value; }
        }
        public Edge nextEdge;
        public bool flag;
        public VisualizedEdgePro imageInfo;
        public GameObject normalLineImage;
        public bool selected {
            set { flag = value; }
            get { return flag; }
        }
        
        public Edge()
        {
            startNode = endNode = null;
            value = 0;
        }
        public Edge(GraphNode _startNode, GraphNode _endNode, float _length)
        {
            startNode = _startNode;
            endNode = _endNode;
            length = _length;
        }
        public bool HasReverseEdge()
        {
            return graph.HasReverseEdge(this);
        }
        override public void UpdateValue(float value)
        {
            base.UpdateValue(value);
            graph.UpdateEdgeValue(this, value);   
        }
    }
    public class Graph
    {
        public GameObject image;
        public AnimationBuffer animationBuffer;
        private const float inf = (float)2e9;
        public GraphNode[] nodes;
        private bool[] used;

        private bool[,] g;
        public bool directed;
        private float[,] d;
        private int size;
        private int edgeCount;
        bool[,] vis;
        public VisualizedPointer pointer_cur;
        public Matrix adjacentMatrix;
        
        public Graph(int _size = 100, bool _directed = false)
        {
            size = _size;
            edgeCount = 0;
            directed = _directed;
            nodes = new GraphNode[size];
            used = new bool[size];
            queue = new GraphNode[size];
            g = new bool[size, size];
            d = new float[size, size];
            vis = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                used[i] = false;
                for (int j = 0; j < size; j++)
                {
                    g[i, j] = false;
                    d[i, j] = inf;
                }
                
            }
        }

        int GetNewIndex()
        {
            for (int i = 0; i < size; i++)
                if (!used[i])
                {
                    used[i] = true;
                    Console.WriteLine("Get new id: {0}",i);
                    return i;
                }
            Console.WriteLine("No availble index now!");
            return -1;
        }

        void DiscardIndex(int index) {
            used[index] = false;
        }
        public void RefreshMatrix()
        {
            if (adjacentMatrix == null) return;
            int maxValidSize;
            for (maxValidSize = size; maxValidSize > 1; maxValidSize--)
                if (nodes[maxValidSize - 1] != null) break;
            if (maxValidSize < 5) adjacentMatrix.fixPanelSize = false;
            else adjacentMatrix.fixPanelSize = true;
            adjacentMatrix.Refresh(maxValidSize , maxValidSize, d);
        }
        public bool AddNode(GraphNode newNode)
        {
            int index = GetNewIndex();
            if (index == -1) return false;
            newNode.id = index;
            newNode.animationBuffer = this.animationBuffer;
            Debug.Log("newNode.animationBuffer:" + newNode.animationBuffer);
            if (string.IsNullOrEmpty(newNode.name))
                newNode.name = newNode.id.ToString();
            Console.WriteLine("Add new node: " + newNode.name);
            newNode.graph = this;
            nodes[index] = newNode;
            newNode.SetText(newNode.name);
            newNode.PopOut();
            RefreshMatrix();
            return true;
        }
        public void UpdateEdgeValue(Edge edge, float value)
        {
            int u = edge.startNode.id, v = edge.endNode.id;
            d[u, v] = value;
            if (adjacentMatrix != null) adjacentMatrix.ChangeText(u, v, value.ToString("f0"));
            if (!directed) {
                d[v, u] = value;
                if (adjacentMatrix != null) adjacentMatrix.ChangeText(v, u, value.ToString("f0"));
            }
        }
        public GraphNode GetNode(int index)
        {
            if (index < 0 || index >= size)
            {
                Console.WriteLine("The index of the node to get is out of range!");
                return null;
            }

            if (nodes[index] == null)
            {
                Console.WriteLine("Cannot find such a node with index of {0}", index);
                return null;
            }

            return nodes[index];
        }
        public GraphNode GetNode(string name)
        {
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    if (nodes[i].name == name)
                        return nodes[i];
                }

            Console.WriteLine("Cannot find such a node with name of {0}!", name);
            return null;
        }
        public void DeleteNode(int index)
        {
            if (index < 0 || index >= size)
            {
                Console.WriteLine("The index of the node to delete is out of range!");
                return;
            }

            if (nodes[index] == null)
            {
                Console.WriteLine("Cannot find such a node in the graph!(index = {0})", index);
                return;
            }
            DeleteNode(nodes[index]);
        }
        public void DeleteNode(GraphNode node)
        {
            if (node.graph != this)
            {
                Console.WriteLine("Cannot find such a node in the graph!(Node:{0})",node.name);
                return;
            }
            node.graph = null;
            nodes[node.id] = null;
            Console.WriteLine("Delete Node: {0} successfully.", node.name);
            DiscardIndex(node.id);
            node.Destroy();
            for (Edge edge = node.firstEdge; edge != null; edge = edge.nextEdge)
                edge.Destroy(true);
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    GraphNode curNode = nodes[i];
                    Edge firstNotNullEdge = null, lastEdge = null;
                    for (Edge edge = curNode.firstEdge; edge != null; edge = edge.nextEdge)
                    {
                        if (edge.endNode == node)
                        {
                            if (lastEdge != null) lastEdge.nextEdge = edge.nextEdge;
                            if (directed) edge.Destroy(true);
                        }
                        else {
                            lastEdge = edge;
                            if (firstNotNullEdge == null) firstNotNullEdge = edge;
                        }
                    }
                    curNode.firstEdge = firstNotNullEdge;
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                if (i == node.id || j == node.id)
                {
                    g[i, j] = false;
                    d[i, j] = inf;
                }
            RefreshMatrix();
        }

        public void ListNodes()
        {
            
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    Console.Write("{0}:{1} ", used[i], nodes[i].name);
                }
                else Console.Write("{0} ",used[i]);
            Console.WriteLine();
        }

        private bool MyAddEdge(int i, int j, ref Edge edgeInfo, GameObject edgeImage, float dist = 0)
        {
            if (nodes[i] == null || nodes[j] == null)
            {
                Console.WriteLine("Cannot add edge with nonexistent node!");
                return false;
            }
            if (g[i, j])
            {
                Console.WriteLine("There have already been an edge from Node: {0} to Node: {1}", nodes[i].name, nodes[j].name);
                return false;
            }
            g[i, j] = true;
            d[i, j] = dist;
            Edge newEdge = new Edge(nodes[i], nodes[j], dist);

            newEdge.graph = this;
            newEdge.image = edgeImage;
            edgeImage.GetComponent<VisualizedEdgePro>().info = newEdge;
            newEdge.normalLineImage = edgeImage.GetComponent<VisualizedEdgePro>().normalLine.gameObject;
           // newEdge.imageInfo = edgeImage.GetComponent<VisualizedEdgePro>();
            newEdge.animationBuffer = this.animationBuffer;
            newEdge.colors = edgeImage.GetComponent<VisualizedEdgePro>().colors;
            newEdge.nextEdge = nodes[i].firstEdge;
            newEdge.UpdateValue(dist);
            edgeInfo = newEdge;
            nodes[i].firstEdge = newEdge;
            edgeCount++;
            RefreshMatrix();
            return true;
        }
        public bool HasReverseEdge(Edge edge)
        {
            if (!directed) return false;
            int u = edge.startNode.id, v = edge.endNode.id;
            if (u < 0 || u >= size) return false;
            if (v < 0 || v >= size) return false;
            return g[v, u];
        }
        public bool AddEdge(int i, int j, ref Edge edgeInfo, GameObject edgeImage = null, float dist = 0)
        {
            if (nodes[i] == null || nodes[j] == null)
            {
                Console.WriteLine("Cannot add edge with nonexistent node!");
                return false;
            }
            if (g[i, j])
            {
                Console.WriteLine("There have already been an edge from [Node: {0}] to [Node: {1}]", nodes[i].name, nodes[j].name);
                return false;
            }
            bool status = true;
            status &= MyAddEdge(i, j, ref edgeInfo, edgeImage, dist);
            if (!directed) status &= MyAddEdge(j, i, ref edgeInfo, edgeImage, dist);
            return status;
        }
        
        bool MyDeleteEdge(int i, int j, bool destroyImage = false)
        {
            g[i, j] = false;
            d[i, j] = inf;
            Edge lastEdge = null;
            for (Edge edge = nodes[i].firstEdge; edge != null; lastEdge = edge, edge = edge.nextEdge)
                if (edge.endNode == nodes[j])
                {
                    if (destroyImage)
                        edge.Destroy();
                    if (lastEdge == null) nodes[i].firstEdge = edge.nextEdge;
                    else lastEdge.nextEdge = edge.nextEdge;
                    Console.WriteLine("Delete the edge from [Node:{0}] to [Node:{1}]", nodes[i].name, nodes[j].name);
                    edgeCount--;
                    RefreshMatrix();
                    return true;
                }

            return false;
        }
        public void DeleteEdge(int i, int j)
        {
            if (!g[i, j] || nodes[i] == null || nodes[j] == null)
            {
                Console.WriteLine("Cannot add an edge with nonexistent node!");
                return;
            }

            MyDeleteEdge(i, j, true);
            if (!directed) MyDeleteEdge(j, i);
        }

        bool MyEditEdge(int i, int j, float newLength)
        {
            d[i, j] = newLength;
            for (Edge edge = nodes[i].firstEdge; edge != null; edge = edge.nextEdge)
                if (edge.endNode == nodes[j])
                {
                    edge.length = newLength;
                    RefreshMatrix();
                    return true;
                }

            return false;
        }

        public bool EditEdge(int i, int j, float newLength)
        {
            if (g[i, j] == false || nodes[i] == null || nodes[j] == null)
            {
                Console.WriteLine("Cannot Find the edge ({0}, {1}).", i, j);
                return false;
            }

            MyEditEdge(i, j, newLength);
            if (!directed) MyEditEdge(j, i, newLength);
            return true;
        }
        
        private void Wait(float sec, bool useSetting = true)
        {
            animationBuffer.Add(new WaitAnimatorInfo(image, sec, useSetting));
        }
        void PointerAppear(GameObject pointer, bool block = false)
        {
            if (pointer == null) return;
            PopAnimatorInfo info = new PopAnimatorInfo(pointer, PopAnimator.Type.Appear);
            info.block = block;
            animationBuffer.Add(info);
        }
        void ChangePointerPos(GameObject pointer, Vector2 newPos, bool animated = true)
        {
            if (pointer == null) return;
            Vector2 offset = pointer.GetComponent<VisualizedPointer>().offset;
            UpdatePosAnimatorInfo info = new UpdatePosAnimatorInfo(pointer, MyMath.LocaltoWorldPosition(pointer.transform, newPos + offset), animated);
            info.block = true;
            animationBuffer.Add(info);
        }
        void PointerDisappear(GameObject pointer, bool block = false)
        {
            if (pointer == null) return;
            PopAnimatorInfo info = new PopAnimatorInfo(pointer, PopAnimator.Type.Disappear);
            info.block = block;
            animationBuffer.Add(info);
        }
        public void ResetStatus()
        {
            for (int i = 0; i < size; i++)
                if (nodes[i] != null) {
                    nodes[i].visited = false;
                    nodes[i].SetColor(Palette.Normal);
                    for (Edge edge = nodes[i].firstEdge; edge != null; edge = edge.nextEdge)
                        edge.SetColor(Palette.Normal);
                    nodes[i].UpdateName(nodes[i].name);
                }
        }
        public void DFS(GraphNode startNode)
        {
            ResetStatus();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    vis[i, j] = false;
            Console.WriteLine();
            Console.Write("DFS: ");
            ChangePointerPos(pointer_cur.gameObject, new Vector2(startNode.x, startNode.y), false);
            PointerAppear(pointer_cur.gameObject);
            MyDFS(startNode, null);
            Wait(-1);
            PointerDisappear(pointer_cur.gameObject);
        }
        private void MyDFS(GraphNode cur, Edge lastEdge)
        {
            cur.visited = true;
            ChangePointerPos(pointer_cur.gameObject, new Vector2(cur.x, cur.y));
            cur.Highlight(true, Palette.Emphasize);

            if (lastEdge != null) lastEdge.SetColor(Palette.Visited);

            Console.Write("[Node:{0}] ",cur.name);
            Wait(1);
            for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
            {
                //Debug.Log("edge.animationBuffer : " + edge.animationBuffer);
                if (vis[cur.id, edge.endNode.id]) continue;
                //if (edge.endNode == from) continue;

                edge.Highlight(true, Palette.Emphasize, false, true, edge.normalLineImage);
                vis[cur.id, edge.endNode.id] = true;
                if (!directed) vis[edge.endNode.id, cur.id] = true;
                
                if (!edge.endNode.visited)
                {
                    Wait(1);
                    cur.SetColor(Palette.Visited);
                    MyDFS(edge.endNode, edge);
                    ChangePointerPos(pointer_cur.gameObject, new Vector2(cur.x, cur.y));
                    Wait(0.5f);
                }
                else Wait(0.5f);
            }
            Wait(0.5f);
            cur.SetColor(Palette.Visited);
        }

        private GraphNode[] queue;
        public void BFS(GraphNode startNode)
        {
            Console.WriteLine();
            Console.Write("BFS: ");
            for (int i = 0; i < size; i++)
                if (nodes[i] != null) {
                    nodes[i].visited = false;
                    nodes[i].backTrace = null;
                }
            ResetStatus();

            ChangePointerPos(pointer_cur.gameObject, new Vector2(startNode.x, startNode.y), false);
            PointerAppear(pointer_cur.gameObject);

            int head = 0, tail = 0, count = 1;
            queue[head] = startNode;
            startNode.visited = true;
            while (count > 0)
            {
                GraphNode cur = queue[head];
                if (cur != startNode) PointerDisappear(pointer_cur.gameObject, true);
                ChangePointerPos(pointer_cur.gameObject, new Vector2(cur.x, cur.y), false);
                PointerAppear(pointer_cur.gameObject);
                cur.Highlight(true, Palette.Current, true);
                cur.backTrace?.Highlight(true, Palette.Visited, false, true, cur.backTrace.normalLineImage);
                Console.Write("[Node:{0}] ", cur.name);
                head = (head + 1) % size;
                count--;
                Wait(1f);
                for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
                {
                    edge.Highlight(true, Palette.Emphasize, false, true, edge.normalLineImage);
                    if (!edge.endNode.visited)
                    {
                        edge.endNode.visited = true;
                        edge.endNode.backTrace = edge;
                        tail = (tail + 1) % size;
                        count++;
                        queue[tail] = edge.endNode;
                        edge.endNode.Highlight(true, Palette.Emphasize);
                        Wait(1f);
                    }
                    else {
                        Wait(0.5f);
                    }
                }
                cur.SetColor(Palette.Visited);
            }
            Wait(1f);
            PointerDisappear(pointer_cur.gameObject);
            Wait(-1);
        }

        public enum MST_Algorithm
        {
            Prim,
            Kruskal
        }
        public float MST(MST_Algorithm algorithm = MST_Algorithm.Kruskal)
        {
            bool foundFirstValidNode = false;
            GraphNode startNode = null;
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    if (!foundFirstValidNode)
                    {
                        foundFirstValidNode = true;
                        startNode = nodes[i];
                    }

                    for (Edge edge = nodes[i].firstEdge; edge != null; edge = edge.nextEdge)
                        edge.selected = false;
                    nodes[i].minWeight = inf;
                    nodes[i].visited = false;
                    nodes[i].backTrace = null;
                }
            
            if (algorithm == MST_Algorithm.Prim)
                return Prim(startNode);
            else 
                return Kruskal();
        }
        
        float Prim(GraphNode startNode)
        {
            Console.Write("Prim result: ");
            float res = 0;
            startNode.minWeight = 0;
            while (true)
            {
                float minWeight = inf;
                GraphNode cur = null;
                for (int i = 0; i < size; i++)
                    if (nodes[i] != null)
                        if (!nodes[i].visited && nodes[i].minWeight < minWeight)
                        {
                            minWeight = nodes[i].minWeight;
                            cur = nodes[i];
                        }

                if (cur == null) break;
                cur.visited = true;
                if (cur.backTrace != null)
                {
                    res += cur.minWeight;
                    cur.backTrace.selected = true;
                }
                for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
                    if (!edge.endNode.visited)
                        if (edge.length < edge.endNode.minWeight)
                        {
                            edge.endNode.minWeight = edge.length;
                            edge.endNode.backTrace = edge;
                        }
            }

            return res;
        }

        private int[] father;

        int FindAncestor(int id)
        {
            if (father[id] != id)
                father[id] = FindAncestor(father[id]);
            return father[id];
        }
        float Kruskal()
        {
            float res = 0;
            Console.WriteLine("Kruskal:");
            SeqList edgeWeights = new SeqList(edgeCount / 2);
            for (int i = 1; i < size; i++)
                if (nodes[i] != null)
                    for (int j = 0; j < i; j++)
                        if (nodes[j] != null)
                            if (g[i, j])
                                edgeWeights.Append(new SeqElement(d[i ,j], new Edge(nodes[i], nodes[j], d[i, j])));
            edgeWeights.Sort(false);
            father = new int[size];
            for (int i = 0; i < size; i++) father[i] = i;
            for (int i = 0; i < edgeWeights.Size(); i++)
            {
                Edge cur = (Edge)edgeWeights.GetElement(i).myObject;
                Console.WriteLine("({0}, {1}, {2})", cur.startNode.name, cur.endNode.name, cur.length);
                int fx = FindAncestor(cur.startNode.id);
                int fy = FindAncestor(cur.endNode.id);
                if (fx == fy) continue;
                res += cur.length;
                cur.selected = true;
                father[fy] = fx;
            }

            father = null;
            return res;
        }

        public float ShortestPath(GraphNode startNode, GraphNode endNode)
        {
            return Dijkstra(startNode, endNode);
        }
        float Dijkstra(GraphNode startNode, GraphNode endNode)
        {
            ResetStatus();
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    nodes[i].UpdateValue(inf);
                    for (Edge edge = nodes[i].firstEdge; edge != null; edge = edge.nextEdge)
                        edge.selected = false;
                    nodes[i].visited = false;
                    nodes[i].backTrace = null;
                }
            startNode.UpdateValue(0);
            ChangePointerPos(pointer_cur.gameObject, new Vector2(startNode.x, startNode.y), false);
            PointerAppear(pointer_cur.gameObject, true);

            while (true)
            {
                float minDist = inf;
                GraphNode cur = null;
                for (int i = 0; i < size; i++)
                    if (nodes[i] != null)
                        if (!nodes[i].visited && nodes[i].minDist < minDist)
                        {
                            minDist = nodes[i].minDist;
                            cur = nodes[i];
                        }

                if (cur == null) break;
                if (cur != startNode)
                {
                    PointerDisappear(pointer_cur.gameObject, true);
                    ChangePointerPos(pointer_cur.gameObject, new Vector2(cur.x, cur.y), false);
                    PointerAppear(pointer_cur.gameObject, false);
                }
                cur.visited = true;
                cur.Highlight(true, Palette.Current);
                Wait(1f);
                if (cur.backTrace != null)
                    cur.backTrace.selected = true;
                for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
                    if (!edge.endNode.visited)
                    {
                        GraphNode otherNode = edge.endNode;
                        if (cur.minDist + edge.length < otherNode.minDist)
                        {
                            otherNode.Highlight(true, Palette.Update);
                            edge.Highlight(false, Palette.BestEdge, false, true);
                            otherNode.UpdateValue(cur.minDist + edge.length);
                            otherNode.backTrace?.SetColor(Palette.VisitedEdge);
                            otherNode.backTrace = edge;
                        }
                        else edge.Highlight(false, Palette.VisitedEdge, false, true);
                        Wait(1f);
                        otherNode.SetColor(Palette.Emphasize);
                    }
                cur.SetColor(Palette.Visited);
                Wait(1f);
            }
            PointerDisappear(pointer_cur.gameObject);
            Wait(-1);
            if (endNode != null)  return endNode.minDist;
            return 0;
        }
    
        public void TopologicalSort()
        {
            if (!directed) {
                Debug.Log("Cannot run Topological Sort on undirected graph.");
                return;
            }
            Queue<GraphNode> que = new Queue<GraphNode>();
            for (int i = 0; i < size; i++)
                if (nodes[i] != null) {
                    nodes[i].visited = false;
                    nodes[i].backTrace = null;
                    nodes[i].indgr = 0;
                }
            ResetStatus();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                if (g[i, j]) nodes[j].indgr++;
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    nodes[i].UpdateValue(nodes[i].indgr);
                    if (nodes[i].indgr == 0)
                        que.Enqueue(nodes[i]);
                }
            bool firstStep = true;
            int curRank = 0;
            while (que.Count > 0)
            {
                GraphNode cur = que.Dequeue();
                if (!firstStep) PointerDisappear(pointer_cur.gameObject, true);
                else firstStep = false;
                ChangePointerPos(pointer_cur.gameObject, new Vector2(cur.x, cur.y), false);
                PointerAppear(pointer_cur.gameObject, false);
                cur.Highlight(true, Palette.Current);
                curRank++;
                cur.UpdateRank(curRank);
                Wait(1f);
                for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
                {
                    GraphNode otherNode = edge.endNode;
                    edge.SetColor(Palette.Current);
                    Wait(0.5f);
                    otherNode.indgr--;
                    edge.SetColor(Palette.HidedEdge);
                    otherNode.Highlight(true, Palette.Update);
                    otherNode.UpdateValue(otherNode.indgr);
                    if (otherNode.indgr == 0)
                    {
                        Wait(0.5f);
                        que.Enqueue(otherNode);
                        otherNode.Highlight(true, Palette.Emphasize);
                    }
                    Wait(1f);
                    if (otherNode.indgr > 0) otherNode.SetColor(Palette.Normal);
                }
                cur.SetColor(Palette.Normal);
            }
            PointerDisappear(pointer_cur.gameObject, true);
            Wait(-1);
            ResetStatus();
        }
        public GraphData ConvertToData()
        {
            GraphData res = new GraphData();
            res.size = this.size;
            res.directed = this.directed;
            res.nodeRegistered = new bool[this.size];
            res.names = new string[this.size];
            res.values = new float[this.size];
            res.pos = new Vector2[this.size];

            for (int i = 0; i < size; i++)
                if (nodes[i] == null) {
                    res.nodeRegistered[i] = false;
                    res.names[i] = "";
                }
                else {
                    res.nodeRegistered[i] = true;
                    res.names[i] = nodes[i].name;
                    res.values[i] = nodes[i].value;
                    res.pos[i] = nodes[i].image.gameObject.transform.localPosition;
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (g[i, j]) res.edges.Add(new EdgeData(i, j, d[i, j]));
            return res;
        }
        public string ConvertToJsonData()
        {
            GraphData data = ConvertToData();
            string res = JsonUtility.ToJson(data);
            return res;
        }
        public void BuildFromJson(string jsonData)
        {
            GraphData data = JsonUtility.FromJson<GraphData>(jsonData);
        }
    }
    [Serializable]
    public class EdgeData{
        public int u, v;
        public float value;
        public EdgeData(int _u, int _v, float _value)
        {
            this.u = _u;
            this.v = _v;
            this.value = _value;
        }
    }
    public class GraphData{
        public int size;
        public bool directed;
        public bool[] nodeRegistered;
        public string[] names;
        public float[] values;
        public Vector2[] pos;
        public List<EdgeData> edges = new List<EdgeData>();

    }