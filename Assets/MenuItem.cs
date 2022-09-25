using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    // Start is called before the first frame update
    public float parentScaleFactor;
    private float scaleFactor;
    private Vector3 scale0;
    Vector2 mouseLastPosition;
    void Start()
    {
        scaleFactor = parentScaleFactor = 1;
        scale0 = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Render();
        CheckMouseClick();
    }
    void Render()
    {
        transform.localScale = scale0 * scaleFactor * parentScaleFactor;
    }
    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseLastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Distance(Input.mousePosition, mouseLastPosition) < 0.01f)
                ;
        }
    }
    float Distance(Vector2 A, Vector2 B) {
        return Mathf.Sqrt( (A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y) );
    }
}
