using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    // Start is called before the first frame update
    AnimationBuffer animationBuffer;
    [SerializeField]
    GameObject rootObject;
    Vector2 lastMousePos, curMousePos;
    Camera mainCam;
    bool isDragging;
    void Awake()
    {
        gameObject.AddComponent<PopAnimator>();
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        isDragging = false;
    }
    virtual protected void Start()
    {
        mainCam = Global.mainCamera;
        lastMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    // Update is called once per frame
    virtual protected void Update()
    {
        curMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (isDragging)
        {
            rootObject.transform.position += (Vector3)(curMousePos - lastMousePos);
        }
        lastMousePos = curMousePos;
    }
    void OnMouseEnter()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut));
    }
    void OnMouseExit()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack));
    }
    void OnMouseDown()
    {
        isDragging = true;
    }
    void OnMouseUp()
    {
        isDragging = false;
    }
}
