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
        public float length {
            set { this.value = value; }
            get { return this.value; }
        }
        public Edge nextEdge;
        public bool flag;
        public VisualizedEdgePro imageInfo;
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
            Console.WriteLine("Delete Node: {0} successfully.", nodes[index].name);
            nodes[index].graph = null;
            DiscardIndex(index);
            nodes[index] = null;
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

        private bool MyAddEdge(int i, int j, GameObject edgeImage, float dist = 0)
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
            newEdge.image = edgeImage;
           // newEdge.imageInfo = edgeImage.GetComponent<VisualizedEdgePro>();
            newEdge.animationBuffer = this.animationBuffer;
            newEdge.colors = edgeImage.GetComponent<VisualizedEdgePro>().colors;
            newEdge.nextEdge = nodes[i].firstEdge;
            newEdge.UpdateValue(dist);
            nodes[i].firstEdge = newEdge;
            edgeCount++;
            return true;
        }

        public bool AddEdge(int i, int j, GameObject edgeImage = null, float dist = 0)
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
            status &= MyAddEdge(i, j, edgeImage, dist);
            if (!directed) status &= MyAddEdge(j, i, edgeImage, dist);
            return status;
        }
        
        bool MyDeleteEdge(int i, int j)
        {
            g[i, j] = false;
            d[i, j] = inf;
            Edge lastEdge = null;
            for (Edge edge = nodes[i].firstEdge; edge != null; lastEdge = edge, edge = edge.nextEdge)
                if (edge.endNode == nodes[j])
                {
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

            MyDeleteEdge(i, j);
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
        public void DFS(GraphNode startNode)
        {
            for (int i = 0; i < size; i++)
                if (nodes[i] != null) nodes[i].visited = false;
            Console.WriteLine();
            Console.Write("DFS: ");
            MyDFS(startNode, null, null);
        }
        private void MyDFS(GraphNode cur, GraphNode from, Edge lastEdge)
        {
            cur.visited = true;
            cur.Highlight(true, Palette.Emphasize);
            if (lastEdge != null) lastEdge.SetColor(Palette.Visited);
            Wait(1f);
            Console.Write("[Node:{0}] ",cur.name);
            for (Edge edge = cur.firstEdge; edge != null; edge = edge.nextEdge)
            {
                //Debug.Log("edge.animationBuffer : " + edge.animationBuffer);
                if (edge.endNode == from) continue;
                edge.Highlight(true, Palette.Emphasize, true);
                if (!edge.endNode.visited)
                {
                    cur.SetColor(Palette.Visited);
                    MyDFS(edge.endNode, cur, edge);
                }
                Wait(1f);
            }
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
            edgeWeights.Sort(0);
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