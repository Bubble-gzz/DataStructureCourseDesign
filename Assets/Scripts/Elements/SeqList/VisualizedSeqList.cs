using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VisualizedSeqList : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Color selectedColor, hoveringColor;
    public bool selected;
    public ListLayOutManager layoutManager;
    [SerializeField]
    public bool selectable;
    public SeqList list;
    [SerializeField]
    GameObject visualizedSeqElementPrefab;
    [SerializeField]
    GameObject visualizedPointerPrefab;
    [SerializeField]
    GameObject appendButtonPrefab;
    int debugCount;
    public AnimationBuffer animationBuffer;
    string listName = "sampleList";
    [SerializeField]
    float defaultInterval = 0.3f;
    [SerializeField]
    bool hasAppendButton = true;
    public bool freezeInsertButton;
    MyCollider myCollider;
    void Awake()
    {
        list = new SeqList(); 
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<WaitAnimator>();
        gameObject.AddComponent<UpdatePosAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();
        list.animationBuffer = animationBuffer;
        list.image = gameObject;
        list.x = transform.position.x;
        list.y = transform.position.y;
        list.pointer_i = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        list.pointer_i.SetText("i");
        list.pointer_j = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        list.pointer_j.SetText("j");
        list.pointer_pivot = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        list.pointer_pivot.SetText("p");
        list.pointer_l = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        list.pointer_l.SetText("l");
        list.pointer_r = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        list.pointer_r.SetText("r");
        myCollider = GetComponentInChildren<MyCollider>();
        debugCount = 0;
        selected = false;
        freezeInsertButton = false;
    }
    void Start()
    {
        if (myCollider != null)
        {
            if (myCollider.GetComponent<ChangeColorAnimator>() != null) myCollider.SetColor(new Color(0,0,0,0));
        }
        //Debug.Log("SeqList animationBuffer : " + animationBuffer.Name);
        StartCoroutine(_CreateAppendButton());
    }
    IEnumerator _CreateAppendButton()
    {
        yield return new WaitForSeconds(0.4f);
        if (hasAppendButton) {
            SeqElement newElement = new SeqElement();
            VisualizedSeqElement newVisualizedElement = 
            Instantiate(appendButtonPrefab, transform).GetComponent<VisualizedSeqElement>();
            newElement.image = newVisualizedElement.gameObject;
            newElement.colors = newVisualizedElement.colors;

            newVisualizedElement.SetText("+");
            newVisualizedElement.info = newElement;
            newVisualizedElement.list = this;
            newVisualizedElement.interval = defaultInterval;
            list.Append(newElement, false);
        }

    }
    void Update()
    {
        list.x = transform.position.x;
        list.y = transform.position.y;

        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            Append(Random.Range(1,50));
            //debugCount++;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Insert(1, Random.Range(1,50));
            //debugCount++;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Delete(0);
        }
        */
        if (Input.GetKeyDown(KeyCode.S))
        {
            Sort();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(list.ConvertToJsonData());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData(listName);
        }
    }
    public bool IsOrdered()
    {
        return list.IsOrdered();
    }
    SeqElement NewElement(float value = 0)
    {
        SeqElement newElement = new SeqElement();
        VisualizedSeqElement newVisualizedElement = 
        Instantiate(visualizedSeqElementPrefab, transform).GetComponent<VisualizedSeqElement>();
        newElement.value = value;
        newElement.image = newVisualizedElement.gameObject;
        newElement.colors = newVisualizedElement.colors;

        newVisualizedElement.SetText(value.ToString("f0"));
        newVisualizedElement.info = newElement;
        newVisualizedElement.list = this;
        newVisualizedElement.interval = defaultInterval;
        return newElement;
    }
    public void OnClickAppend()
    {
        Append(Random.Range(0,100));
    }
    public SeqElement Append(float value = 0)
    {
        return Insert(list.Size(), value);
    }
    public SeqElement Insert(int pos, float value = 0)
    {
        SeqElement newElement = NewElement(value);
        list.Insert(pos, newElement);  
        return newElement;          
    }
    public void Delete(int pos, bool destroy = true)
    {
        list.Delete(pos, destroy);
    }
    public void RefreshPos()
    {
        list.RefreshPos();
    }
    public void Sort()
    {
        list.Sort();
    }
    public void SaveData()
    {
        string jsonData = list.ConvertToJsonData();
        string path = Application.dataPath + listName + ".data";
        File.WriteAllText(path, jsonData);
    }
    public void LoadData(string listName)
    {
        string path = Application.dataPath + listName + ".data";
        string jsonData = File.ReadAllText(path);
        BuildFromJson(jsonData);
    }
    public void BuildFromJson(string jsonData)
    {
        SeqListData data = JsonUtility.FromJson<SeqListData>(jsonData);
        UpdatePos(data.pos);
        list.BuildFromJson(jsonData);
        foreach (var elem in data.elems) Append(elem);
    
    }
    public void UpdatePos(Vector2 pos)
    {
        transform.position = pos;
        list.x = transform.position.x;
        list.y = transform.position.y;
    }
    public void Destroy()
    {
        StartCoroutine(_Destory());
    }
    IEnumerator _Destory()
    {
        while (list.Size() > 0) {
            list.Delete(0);
            yield return new WaitForSeconds(0.1f);
        }
        animationBuffer.Add(new SelfDestroyAnimatorInfo(gameObject));
    }
    public void OnMouseEnter()
    {
        if (!selectable) return;
        Global.mouseOverSeqListBar = true;
        if (selected) return;
        myCollider.SetColor(hoveringColor);
    }
    public void OnMouseClick()
    {
        if (!selectable) return;
        if (selected)
        {
            layoutManager.UnSelectList(this);
            OnMouseEnter();
            return;
        }
        myCollider.SetColor(selectedColor);
        layoutManager.SelectList(this);
    }
    public void OnMouseExit()
    {
        if (!selectable) return;
        Global.mouseOverSeqListBar = false;
        if (selected) return;
        HideBar();
    }
    public void HideBar()
    {
        myCollider.SetColor(new Color(1,1,1,0));
    }
}
