using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualizedElement : MonoBehaviour
{
    [SerializeField]
    public List<Color> colors = new List<Color>();
    public bool alive = true;
    public enum Type{
        Normal,
        Ghost,
        AppendButton,
        InsertButton
    }
    [SerializeField]
    protected Type type;
    protected TMP_Text text;
    protected SpriteRenderer sprite;
    protected Canvas canvas;
    public AnimationBuffer animationBuffer;
    protected MyCollider myCollider;
    [SerializeField]
    protected Vector3 panelOffset = new Vector3(0, 3, -1);
    [SerializeField]
    protected GameObject panelPrefab;
    [SerializeField]
    protected bool interactable = true;
    [SerializeField]
    protected bool appearOnCreate = false;
    public DataElement info;
    protected Camera mainCam;
    public float interval, size;
    virtual protected void Awake()
    {
        Transform child = transform.Find("Canvas/Text");
        if (child != null) text = child.GetComponent<TMP_Text>();
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) {
            sprite.enabled = appearOnCreate;
        }
        canvas = transform.Find("Canvas")?.GetComponent<Canvas>();
        if (canvas != null) {
            canvas.enabled = appearOnCreate;
        }
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<ChangeColorAnimator>();
        gameObject.AddComponent<ChangeTextAnimator>();
        gameObject.AddComponent<PopAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();
        gameObject.AddComponent<WaitAnimator>();
    }
    virtual protected void Start()
    {
        mainCam = Global.mainCamera;
        if (type != Type.Ghost) {
            if (interactable) myCollider = transform.Find("OperateArea").GetComponentInChildren<MyCollider>();
            //Debug.Log("operateArea : " + myCollider);
        }
        else myCollider = transform.Find("GhostArea").GetComponent<MyCollider>();
        if (interactable) {
            myCollider.onMouseEnter.AddListener(MyOnMouseEnter);
            myCollider.onMouseExit.AddListener(MyOnMouseExit);
            myCollider.onMouseClick.AddListener(MyOnMouseClick);
        }
    }
    public void SetText(string newText)
    {
        text.text = newText;
    }
    virtual protected void MyOnMouseEnter()
    {
        if (!alive) return;
        if (type == Type.Ghost) 
        {

        }
        else if (type == Type.InsertButton)
        {
            OnInsertButtonEnter();
        }
        else {
            animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut));
   
         }
    }
    virtual protected void MyOnMouseExit()
    {
        if (!alive) return;
        if (type == Type.Ghost)
        {

        }
        else if (type == Type.InsertButton)
        {
            OnInsertButtonExit();
        }
        else {
            animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack));
        }

    }
    virtual protected void OnInsertButtonEnter()
    {

    }
    virtual protected void OnInsertButtonExit()
    {

    }
    virtual protected void MyOnMouseClick()
    {
        if (!alive) return;
        if (type == Type.Ghost) return;
        GameObject newPanel = Instantiate(panelPrefab);
        newPanel.GetComponentInChildren<ElementPanel>().element = this;
        newPanel.transform.position = transform.position + panelOffset;
    }
    virtual public void OnDelete()
    {
    }
}
