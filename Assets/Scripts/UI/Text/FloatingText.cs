using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    GameObject parentObj;
    Camera mainCam;

    [SerializeField]
    Vector2 offset;
    RectTransform rect;
    
    void Start()
    {
        parentObj = transform.parent.gameObject;
        mainCam = Global.mainCamera;
        rect = GetComponent<RectTransform>();
    }

    
    void Update()
    {
        if (parentObj)
        {
            Vector2 pos = (Vector2)mainCam.WorldToScreenPoint(parentObj.transform.position);
            Vector2 refRes;
            refRes = GetComponent<CanvasScaler>().referenceResolution;
            
            pos.x = pos.x * 1.0f / Screen.width * refRes.x;
            pos.y = pos.y * 1.0f / Screen.height * refRes.y;
            pos += offset;
            rect.anchoredPosition = pos;
        }
    }
}
