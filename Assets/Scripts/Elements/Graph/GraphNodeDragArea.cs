using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class GraphNodeDragArea : MonoBehaviour
{
    [SerializeField]
    GameObject rootObject;
    VisualizedNode root;
    public bool mouseEnter, isDragging, isMouseDown;
    Camera mainCam;
    Vector2 lastMousePos, curMousePos, clickMousePos;
    [SerializeField]
    GameObject edgePrefab;
    [SerializeField]
    GameObject panelPrefab;
    [SerializeField]
    Vector3 panelOffset = new Vector3(1,1,-1);
    Initializer initializer;
    void Awake()
    {
        root = rootObject.GetComponent<VisualizedNode>();
        root.dragArea = this;
        mouseEnter = false;
        isDragging = false;
        isMouseDown = false;
    }
    void Start()
    {
        mainCam = Global.mainCamera;
        lastMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        initializer = Global.initializer;
    }

    void Update()
    {
        if (Global.mouseOverUI) {
            isDragging = false;
            isMouseDown = false;
            return;
        }
        MouseCheck();
        PosCheck();
    }
    void MouseCheck()
    {
        DragCheck();
        lastMousePos = curMousePos;
        curMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    void PosCheck()
    {
        if (Global.mouseMode != Global.MouseMode.Move) return;
        if (isDragging) root.transform.position += (Vector3)(curMousePos - lastMousePos);
    }
    void DragCheck()
    {
        if (!isMouseDown || isDragging) return;
        if ((curMousePos - clickMousePos).magnitude < 0.01f) return;
        isDragging = true;
        if (Global.mouseMode == Global.MouseMode.AddEdge)
        {
            VisualizedEdgePro newEdge = Instantiate(edgePrefab, root.graph.transform).GetComponent<VisualizedEdgePro>();
            newEdge.nodes[0] = root.gameObject;
        }
    }
    void OnMouseEnter()
    {
        if (Global.mouseOverUI) return;
        root.animationBuffer.Add(new PopAnimatorInfo(rootObject, PopAnimator.Type.PopOut));
        mouseEnter = true;
        Global.selectedNode = root.gameObject;
    }
    void OnMouseOver()
    {
        if (Global.mouseOverUI) return;
        Global.selectedNode = root.gameObject;
    }
    void OnMouseExit()
    {   
        if (Global.mouseOverUI) return;
        root.animationBuffer.Add(new PopAnimatorInfo(rootObject, PopAnimator.Type.PopBack));
        mouseEnter = false;
        Global.selectedNode = null;
    }
    void OnMouseUp()
    {
        if (Global.mouseOverUI) return;
        isDragging = false;
        isMouseDown = false;
        if (!mouseEnter || (curMousePos - clickMousePos).magnitude > 0.01f) return;
        if (Global.mouseMode == Global.MouseMode.DFS) {
            root.DFS();
            return ;
        }
        root.animationBuffer.Add(new PopAnimatorInfo(root.gameObject, PopAnimator.Type.PopBack));
        GameObject newPanel = Instantiate(panelPrefab);
        newPanel.GetComponentInChildren<NodePanel>().node = root.gameObject;
        newPanel.transform.position = transform.position + (Vector3)panelOffset;
    }
    void OnMouseDown()
    {
        //Debug.Log("mouseOverUI:" + Global.mouseOverUI);
        if (Global.mouseOverUI) return;
        isMouseDown = true;
        clickMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}
