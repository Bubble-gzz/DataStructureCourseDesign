using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizedLinkStack : MonoBehaviour
{
    LinkStack stack;
    [SerializeField]
    GameObject visualizedStackElementPrefab;
    [SerializeField]
    GameObject visualizedPointerPrefab;
    int debugCount;
    AnimationBuffer animationBuffer;
    string stackName = "sampleStack";
    void Start()
    {
        stack = new LinkStack(); 
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        stack.animationBuffer = animationBuffer;
        stack.image = gameObject;
        
        stack.pointer_top = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        stack.pointer_top.SetText("top");

        debugCount = 0;
    }

    void Update()
    {
        stack.pos = transform.position;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Push(Random.Range(0, 100));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pop();
        }
    }
    StackElement NewElement(float value = 0)
    {
        StackElement newElement = new StackElement();
        VisualizedStackElement newVisualizedElement = 
        Instantiate(visualizedStackElementPrefab, transform).GetComponent<VisualizedStackElement>();
        newElement.value = value;
        newElement.image = newVisualizedElement.gameObject;
        newElement.colors = newVisualizedElement.colors;

        newVisualizedElement.SetText(value.ToString("f0"));
        newVisualizedElement.info = newElement;
        newVisualizedElement.stack = this;
        return newElement;
    }
    public void Push(float value = 0)
    {
        StackElement newElement = NewElement(value);
        stack.Push(newElement);
    }
    public void Pop()
    {
        Debug.Log("Pop : " + stack.Pop()?.value);
    }
    void UpdataPos(Vector2 pos)
    {
        transform.position = pos;
    }
}
