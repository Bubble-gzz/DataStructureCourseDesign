using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    [SerializeField]
    float minViewSize = 0.3f, maxViewSize = 1.2f, scrollSpeed = 0.05f;
    Vector2 lastMousePos, curMousePos;
    bool isDragging;
    Camera mainCam;
    void Start()
    {
        mainCam = Global.mainCamera;
        lastMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        ViewUpdate();
    }
    void ViewUpdate()
    {
        float delta = Input.mouseScrollDelta.y * scrollSpeed;
        float newSize = Mathf.Clamp(transform.localScale.x + delta, minViewSize, maxViewSize);
        //Debug.Log("mouseScrollDelta: " + delta);
        transform.localScale = new Vector2(newSize, newSize);

        if (Input.GetMouseButtonDown(2)) isDragging = true;
        if (Input.GetMouseButtonUp(2)) isDragging = false;
       
        curMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (isDragging)
        {
            Vector2 deltaPos = curMousePos - lastMousePos;
            transform.position += (Vector3)deltaPos;
        }
        lastMousePos = curMousePos;
    }
}
