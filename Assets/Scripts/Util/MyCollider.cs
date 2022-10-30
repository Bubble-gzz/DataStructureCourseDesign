using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyCollider : MonoBehaviour
{
    enum Shape{
        Rectangle,
        Circle
    }
    [SerializeField]
    Shape shape;
    float height, width;
    float radius;
    Camera mainCam;
    bool mouseOver, mouseDown, lastMouseOver;
    Vector2 lastClickPos;
    public UnityEvent onMouseEnter = new UnityEvent();
    public UnityEvent onMouseExit = new UnityEvent();
    public UnityEvent onMouseDown = new UnityEvent();
    public UnityEvent onMouseClick = new UnityEvent();
    public UnityEvent onMouseUp = new UnityEvent();
    AnimationBuffer animationBuffer;
    
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<ChangeColorAnimator>();
    }
    void Start()
    {
        mainCam = Global.mainCamera;
        mouseOver = CheckMouseOver();
        mouseDown = false;
        lastClickPos = new Vector2(0, 0);
    }
    void Update()
    {
        lastMouseOver = mouseOver;
        if (Global.mouseOverUI) return;
        mouseOver = CheckMouseOver();
        CheckMouseInOut();
        CheckMouseDown();
        CheckMouseUp();
    }
    bool CheckMouseOver()
    {
        width = gameObject.transform.localScale.x;
        height = gameObject.transform.localScale.y;
        radius = gameObject.transform.localScale.x;

        Vector2 pos = gameObject.transform.position;
        Vector2 mousePos = CurMousePos();
        if (shape == Shape.Rectangle)
        {
            if (mousePos.x < pos.x - width * 0.5f || mousePos.x > pos.x + width * 0.5f) return false;
            if (mousePos.y < pos.y - height * 0.5f || mousePos.y > pos.y + height * 0.5f) return false;
            return true;
        }
        else if (shape == Shape.Circle)
        {
            if ((mousePos - pos).magnitude > radius) return false;
            return true;
        }
        return false;
    }
    void CheckMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && mouseOver) {
            onMouseDown.Invoke();
            mouseDown = true;
            lastClickPos = CurMousePos();
        }
    }
    void CheckMouseUp()
    {
        if (!Input.GetMouseButtonUp(0)) return;
        onMouseUp.Invoke();
        if (mouseOver && mouseDown && (CurMousePos() - lastClickPos).magnitude < 0.1f)
        {
            onMouseClick.Invoke();
        }
        mouseDown = false;
    }
    void CheckMouseInOut()
    {
        if (lastMouseOver && !mouseOver) {
            onMouseExit.Invoke();
        }
        if (!lastMouseOver && mouseOver) {
            onMouseEnter.Invoke();
        }
    }
    Vector2 CurMousePos()
    {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    public void SetColor(Color newColor)
    {
        animationBuffer.Add(new ChangeColorAnimatorInfo(gameObject, newColor));
    }
}
