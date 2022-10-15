using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizedSeqList : MonoBehaviour
{
    // Start is called before the first frame update
    SeqList list;
    [SerializeField]
    GameObject visualizedSeqElementPrefab;
    [SerializeField]
    GameObject visualizedPointerPrefab;
    int debugCount;
    AnimationBuffer animationBuffer;
    [SerializeField]
    float sortDelay;

    void Start()
    {
        list = new SeqList(); 
        animationBuffer = GetComponent<AnimationBuffer>();
        list.animationBuffer = animationBuffer;
        list.image = gameObject;
        
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
    }

    // Update is called once per frame
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
            Sort();
        }
    }
    SeqElement NewElement(float value = 0)
    {
        SeqElement newElement = new SeqElement();
        GameObject newVisualizedElement = Instantiate(visualizedSeqElementPrefab, transform);
        newElement.value = value;
        newElement.image = newVisualizedElement;
        newElement.imageInfo = newVisualizedElement.GetComponent<VisualizedSeqElement>();
        newVisualizedElement.GetComponent<VisualizedSeqElement>().SetText(value.ToString("f0"));
        return newElement;
    }
    void Append(float value = 0)
    {
        SeqElement newElement = NewElement(value);
        list.Append(newElement);
        //Debug.Log(newElement.pos);
    }
    void Insert(int pos, float value = 0)
    {
        SeqElement newElement = NewElement(value);
        list.Insert(pos, newElement);            
    }
    void Delete(int pos, bool destroy = true)
    {
        list.Delete(pos, destroy);
    }
    void Sort()
    {
        list.Sort(sortDelay);
    }
}
