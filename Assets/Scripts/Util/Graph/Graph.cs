using System;
using System.IO;
using System.Runtime.InteropServices;
using DataStructureCourseDesign.SeqListSpace;
using UnityEngine;

    public class GraphNode : DataElement
    {
        public string name;
        public int id;
        
        public Graph graph;
        public Edge firstEdge;
        public Edge backTrace;
        
        public bool flag;
        public float weight;

        public bool visited {
            set { flag = value;  }
            get { return flag;  }
        }
        public float minDist {
            set { weight = value; }
            get { return weight; }
        }
        
        public float minWeight {
            set { weight = value; }
            get { return weight; }
        }

        public GraphNode(string _name = "")
        {
            name = _name;
            x = y = 0;
            firstEdge = null;
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
    }
    public class Graph
    {
        public GameObject image;
        public AnimationBuffer animationBuffer;
        private const float inf = (float)2e9;
        public GraphNode[] nodes;
        private bool[] used;

        private bool[,] g;
        private bool directed;
        private float[,] d;
        private int size;
        private int edgeCount;
        bool[,] vis;
        public VisualizedPointer pointer_cur;
        
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
        public bool AddNode(GraphNode newNode)
        {
            int index = GetNewIndex();
            if (index == -1) return false;
            newNode.id = index;
            newNode.animationBuffer = this.animationBuffer;
            if (string.IsNullOrEmpty(newNode.name))
                newNode.name = newNode.id.ToString();
            Console.WriteLine("Add new node: " + newNode.name);
            newNode.graph = this;
            nodes[index] = newNode;
            newNode.SetText(newNode.name);
            newNode.PopOut();
            
            return true;
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
        
        private void Wait(float sec)
        {
            animationBuffer.Add(new WaitAnimatorInfo(image, sec));
        }
        void PointerAppear(GameObject pointer)
        {
            if (pointer == null) return;
            animationBuffer.Add(new PopAnimatorInfo(pointer, PopAnimator.Type.Appear));
        }
        void ChangePointerPos(GameObject pointer, Vector2 newPos, bool animated = true)
        {
            if (pointer == null) return;
            Vector2 offset = pointer.GetComponent<VisualizedPointer>().offset;
            UpdatePosAnimatorInfo info = new UpdatePosAnimatorInfo(pointer, MyMath.LocaltoWorldPosition(pointer.transform, newPos + offset), animated);
            info.block = true;
            animationBuffer.Add(info);
        }
        void PointerDisappear(GameObject pointer)
        {
            if (pointer == null) return;
            animationBuffer.Add(new PopAnimatorInfo(pointer, PopAnimator.Type.Disappear));
        }
        public void ResetColors()
        {
            for (int i = 0; i < size; i++)
                if (nodes[i] != null) {
                    nodes[i].visited = false;
                    nodes[i].SetColor(Palette.Normal);
                    for (Edge edge = nodes[i].firstEdge; edge != null; edge = edge.nextEdge)
                        edge.SetColor(Palette.Normal);
                }
        }
        public void DFS(GraphNode startNode)
        {
            ResetColors();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    vis[i, j] = false;
            Console.WriteLine();
            Console.Write("DFS: ");
            ChangePointerPos(pointer_cur.gameObject, new Vector2(startNode.x, startNode.y), false);
            PointerAppear(pointer_cur.gameObject);
            MyDFS(startNode, null);
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

                edge.Highlight(true, Palette.Emphasize, true, edge.normalLineImage);
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
                if (nodes[i] != null) nodes[i].visited = false;
            int head = 0, tail = 0, count = 1;
            
            queue[head] = startNode;
            startNode.visited = true;
            while (count > 0)
            {
                GraphNode cur = queue[head];
                Console.Write("[Node:{0}] ", cur.name);
                head = (head + 1) % size;
                count--;
                for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
                    if (!edge.endNode.visited)
                    {
                        edge.endNode.visited = true;
                        tail = (tail + 1) % size;
                        count++;
                        queue[tail] = edge.endNode;
                    }
            }
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
            for (int i = 0; i < size; i++)
                if (nodes[i] != null)
                {
                    for (Edge edge = nodes[i].firstEdge; edge != null; edge = edge.nextEdge)
                        edge.selected = false;
                    nodes[i].minDist = inf;
                    nodes[i].visited = false;
                    nodes[i].backTrace = null;
                }

            startNode.minDist = 0;
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
                cur.visited = true;
                if (cur.backTrace != null)
                    cur.backTrace.selected = true;
                for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
                    if (!edge.endNode.visited)
                    {
                        GraphNode otherNode = edge.endNode;
                        if (cur.minDist + edge.length < otherNode.minDist)
                        {
                            otherNode.minDist = cur.minDist + edge.length;
                            otherNode.backTrace = edge;
                        }
                    }
            }
            return endNode.minDist;
        }
    }