using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class VisualizedSeqElement : MonoBehaviour
{
    [SerializeField]
    public List<Color> colors = new List<Color>();
    bool isGhost = false;
    public bool alive = true;
    public enum ColorType{
        Normal,
        Pivot,
        Pointed
    }
    public string initialText;
    protected TMP_Text text;
    protected SpriteRenderer sprite;
    protected Canvas canvas;
    public AnimationBuffer animationBuffer;
    MyCollider myCollider;
    public SeqElement info;
    public VisualizedSeqList list;
    Vector3 panelOffset = new Vector3(0, 3, -1);
    [SerializeField]
    GameObject panelPrefab;
    void Awake()
    {
        Transform child = transform.Find("Canvas/Text");
        text = child.GetComponent<TMP_Text>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
        animationBuffer = GetComponent<AnimationBuffer>();
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<ChangeColorAnimator>();
        gameObject.AddComponent<ChangeTextAnimator>();
        gameObject.AddComponent<PopAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();
        gameObject.AddComponent<WaitAnimator>();
    }
    void Start()
    {
        if (!isGhost) myCollider = transform.Find("OperateArea").GetComponentInChildren<MyCollider>();
        else myCollider = transform.Find("GhostArea").GetComponent<MyCollider>();
        myCollider.onMouseEnter.AddListener(MyOnMouseEnter);
        myCollider.onMouseExit.AddListener(MyOnMouseExit);
        myCollider.onMouseClick.AddListener(MyOnMouseClick);
    }
    public void SetText(string newText)
    {
        text.text = newText;
    }
    void MyOnMouseEnter()
    {
        if (!alive) return;
        if (isGhost) 
        {

        }
        else {
            animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut));
   
         }
    }
    void MyOnMouseExit()
    {
        if (!alive) return;
        if (isGhost)
        {

        }
        else {
            animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack));
        }

    }
    void MyOnMouseClick()
    {
        if (isGhost) return;
        GameObject newPanel = Instantiate(panelPrefab);
        newPanel.GetComponentInChildren<ElementPanel>().element = gameObject;
        newPanel.transform.position = transform.position + panelOffset;
    }
    public void OnDelete()
    {
        list.Delete(info.pos, true);
    }
}
