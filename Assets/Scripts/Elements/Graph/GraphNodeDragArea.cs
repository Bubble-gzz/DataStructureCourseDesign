using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNodeDragArea : MonoBehaviour
{
    [SerializeField]
    GameObject rootObject;
    VisualizedNode root;
    public bool mouseEnter, isDragging;
    Camera mainCam;
    Vector2 lastMousePos, curMousePos;
    [SerializeField]
    GameObject edgePrefab;
    void Awake()
    {
        root = rootObject.GetComponent<VisualizedNode>();
        root.dragArea = this;
        mouseEnter = false;
        isDragging = false;
    }
    void Start()
    {
        mainCam = Global.mainCamera;
        lastMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        MouseCheck();
        PosCheck();
    }
    void MouseCheck()
    {
        if (Input.GetMouseButtonDown(0))
            if (mouseEnter) OnClicked();
        if (Input.GetMouseButtonUp(0))
            isDragging = false;
        lastMousePos = curMousePos;
        curMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    void PosCheck()
    {
        if (Global.mouseMode != Global.MouseMode.Move) return;
        if (isDragging) root.transform.position += (Vector3)(curMousePos - lastMousePos);
    }
    void OnMouseEnter()
    {
        root.animationBuffer.Add(new PopAnimatorInfo(rootObject, PopAnimator.Type.PopOut));
        mouseEnter = true;
        Global.selectedNode = root.gameObject;
    }
    void OnMouseOver()
    {
        Global.selectedNode = root.gameObject;
    }
    void OnMouseExit()
    {
        root.animationBuffer.Add(new PopAnimatorInfo(rootObject, PopAnimator.Type.PopBack));
        mouseEnter = false;
        Global.selectedNode = null;
    }
    void OnClicked()
    {
        isDragging = true;
        if (Global.mouseMode == Global.MouseMode.DFS) {
            root.DFS();
            return ;
        }
        if (Global.mouseMode != Global.MouseMode.AddEdge) return;
        VisualizedEdgePro newEdge = Instantiate(edgePrefab).GetComponent<VisualizedEdgePro>();
        newEdge.nodes[0] = root.gameObject;
    }
}
