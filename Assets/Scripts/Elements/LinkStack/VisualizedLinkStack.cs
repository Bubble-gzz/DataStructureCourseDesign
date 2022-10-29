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
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<WaitAnimator>();

        stack = new LinkStack(); 
        stack.animationBuffer = animationBuffer;
        stack.image = gameObject;
        stack.pos = transform.position;
        
        stack.pointer_top = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        stack.pointer_top.SetText("top");
        stack.pointer_top.animationBuffer = animationBuffer;
        stack.pointer_top.offset = new Vector2(-1.2f, 0);
        stack.pointer_top.transform.localScale = new Vector2(0.7f, 0.7f);
        stack.pointer_top.ChangePos(stack.CalcPos(0), false);
        stack.pointer_top.Appear();
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
