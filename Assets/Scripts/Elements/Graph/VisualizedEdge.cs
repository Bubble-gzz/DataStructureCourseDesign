using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualizedEdge : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer dashedLine, normalLine;
    List<Vector2> ends;
    Camera mainCam;
    //bool playingDrawAnimation;
    enum State{
        Hover,
        Drawn
    }
    State state;
    Edge edge;
    public List<GameObject> nodes;
    void Awake()
    {
        dashedLine = transform.Find("DashedLine").GetComponent<LineRenderer>();
        normalLine = GetComponent<LineRenderer>();
        dashedLine.positionCount = 2;
        normalLine.positionCount = 2;
        dashedLine.enabled = true;
        normalLine.enabled = false;

        state = State.Hover;
        ends = new List<Vector2>();
        ends.Add(new Vector2(0, 0));
        ends.Add(new Vector2(0, 0));
        nodes = new List<GameObject>();
        nodes.Add(null); nodes.Add(null);
        //playingDrawAnimation = false;
    }
    void Start()
    {
        mainCam = Global.mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        RefreshEnds();
        RefreshPos();
        RefreshState();
    }
    void RefreshEnds()
    {
        for (int i = 0; i < 2; i++)
        {
            if (nodes[i] != null)
                ends[i] = nodes[i].transform.position;
        }
    }
    void RefreshPos()
    {
        if (state == State.Drawn) {
        }
        if (state == State.Hover)
        {
            ends[1] = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
        for (int i = 0; i < 2; i++)
        {
            dashedLine.SetPosition(i, ends[i]);
            normalLine.SetPosition(i, ends[i]);
        }
    }
    void RefreshState()
    {
        if (state == State.Hover)
        {
            bool flag = true;
            if (Input.GetMouseButtonUp(0))
            {
                flag = Draw();
            } 
            if (Input.GetMouseButton(1) || !flag) Destroy(gameObject);
        }
    }
    bool Draw()
    {
        if (Global.selectedNode == null) return false;
        if (Global.selectedNode == nodes[0]) return false; // no self-loop
        nodes[1] = Global.selectedNode;
        VisualizedNode U = nodes[0].GetComponent<VisualizedNode>();
        VisualizedNode V = nodes[1].GetComponent<VisualizedNode>();
        //Debug.Log("U:" + U + "V:" + V);
        if (!U.graph.AddEdge(U, V, gameObject, ref edge)) return false;
        
        state = State.Drawn;
        RefreshEnds();
        StartCoroutine(DrawAnimation());
        return true;
    }
    IEnumerator DrawAnimation()
    {
        float progress = 0, speed = 7;
        //playingDrawAnimation = true;
        normalLine.enabled = true;
        while (1 - progress > Time.deltaTime * speed)
        {
            progress += Time.deltaTime * speed;
            normalLine.SetPosition(1, Vector2.Lerp(ends[0], ends[1], progress));
            yield return null;
        }
        dashedLine.enabled = false;
        //playingDrawAnimation = false;
    }
    
}
