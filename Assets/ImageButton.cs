using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ImageButton : MonoBehaviour
{
    // Start is called before the first frame update
    AnimationBuffer animationBuffer;
    bool mouseEntered = false;
    bool mouseDown = false;
    Vector2 lastMouseClickPos;
    [SerializeField]
    Camera mainCam;
    virtual protected void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
    }

    virtual protected void Start()
    {
        mainCam = Global.mainCamera;
    }

    virtual protected void Update()
    {
       
    }

    void OnMouseEnter()
    {
        mouseEntered = true;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut));
    }

    void OnMouseExit()
    {
        mouseEntered = false;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack));
    }
    
    void OnMouseDown()
    {
        if (!mouseEntered) return;
        mouseDown = true;
        lastMouseClickPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    void OnMouseUp()
    {
        if (mouseDown)
        {
            Vector2 curMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if ((curMousePos - lastMouseClickPos).magnitude < 0.01f)
                OnMouseClicked();
        }
        mouseDown = false;
    }
    virtual protected void OnMouseClicked()
    {
        
    }
}
