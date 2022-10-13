using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizedSeqList : MonoBehaviour
{
    // Start is called before the first frame update
    SeqList list;
    [SerializeField]
    GameObject visualizedSeqElementPrefab;
    int debugCount;
    AnimationBuffer animationBuffer;
    void Start()
    {
        list = new SeqList();
        animationBuffer = GameObject.Find("AnimationBuffer").GetComponent<AnimationBuffer>();
        list.animationBuffer = animationBuffer;
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
        SeqElement newElement = new SeqElement(value);
        GameObject newVisualizedElement = Instantiate(visualizedSeqElementPrefab, transform);
        newVisualizedElement.GetComponent<VisualizedSeqElement>().value = value;
        newVisualizedElement.GetComponent<VisualizedSeqElement>().animationBuffer = animationBuffer;
        //newVisualizedElement.GetComponent<SpriteRenderer>().enabled = false;
        newElement.image = newVisualizedElement.GetComponent<VisualizedSeqElement>();
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
        list.Sort();
    }
}
