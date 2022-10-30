using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VisualizedSeqList : MonoBehaviour
{
    // Start is called before the first frame update
    SeqList list;
    [SerializeField]
    GameObject visualizedSeqElementPrefab;
    [SerializeField]
    GameObject visualizedPointerPrefab;
    [SerializeField]
    GameObject appendButtonPrefab;
    int debugCount;
    AnimationBuffer animationBuffer;
    string listName = "sampleList";
    [SerializeField]
    float defaultInterval = 0.3f;
    [SerializeField]
    bool hasAppendButton = true;
    void Start()
    {
        list = new SeqList(); 
        animationBuffer = GetComponent<AnimationBuffer>();
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

        debugCount = 0;

        if (hasAppendButton) {
            SeqElement newElement = new SeqElement();
            VisualizedSeqElement newVisualizedElement = 
            Instantiate(appendButtonPrefab, transform).GetComponent<VisualizedSeqElement>();
            newElement.image = newVisualizedElement.gameObject;
            newElement.colors = newVisualizedElement.colors;

            newVisualizedElement.SetText("+");
            newVisualizedElement.info = newElement;
            newVisualizedElement.list = this;
            list.Append(newElement, false);
        }
    }

    void Update()
    {
        list.x = transform.position.x;
        list.y = transform.position.y;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Append(Random.Range(1,50));
            debugCount++;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Insert(1, Random.Range(1,50));
            debugCount++;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Delete(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();//Sort();
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
        return newElement;
    }
    public void Append(float value = 0)
    {
        Insert(list.Size(), value);
    }
    public void Insert(int pos, float value = 0)
    {
        SeqElement newElement = NewElement(value);
        list.Insert(pos, newElement);            
    }
    public void Delete(int pos, bool destroy = true)
    {
        list.Delete(pos, destroy);
    }
    void Sort()
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
        UpdataPos(data.pos);
        list.BuildFromJson(jsonData);
        foreach (var elem in data.elems) Append(elem);
    
    }
    void UpdataPos(Vector2 pos)
    {
        transform.position = pos;
    }
}
