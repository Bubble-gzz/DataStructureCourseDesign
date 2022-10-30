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
    [SerializeField]
    GameObject bracketListPrefab;
    int debugCount;
    AnimationBuffer animationBuffer;
    string stackName = "sampleStack";
    Camera mainCam;
    [SerializeField]
    Vector2 sidePos;
    [SerializeField]
    GameObject messagePrefab;
    VisualizedSeqList list;
    void Start()
    {
        mainCam = Global.mainCamera;

        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        animationBuffer.Name = "LinkStack";
        gameObject.AddComponent<WaitAnimator>();
        gameObject.AddComponent<UpdatePosAnimator>();

        stack = new LinkStack(); 
        stack.animationBuffer = animationBuffer;
        stack.image = gameObject;
        stack.pos = transform.position;
        
        stack.pointer_top = Instantiate(visualizedPointerPrefab, transform).GetComponent<VisualizedPointer>();
        stack.pointer_top.SetText("top");
        stack.pointer_top.animationBuffer = animationBuffer;
        stack.pointer_top.offset = new Vector2(1.2f, 0);
        stack.pointer_top.transform.localScale = new Vector2(0.7f, 0.7f);
        stack.pointer_top.ChangePos(stack.CalcPos(0), false, false);
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
    public void OnClickedPush()
    {
        Push();
    }
    public void OnClickedPop()
    {
        Pop();
    }
    public StackElement Push(float value = 0)
    {
        StackElement newElement = NewElement(value);
        return stack.Push(newElement);
    }
    public void Pop(float delay = 0.24f)
    {
        stack.Pop(delay);
        //Debug.Log("Pop : " + stack.Pop()?.value);
    }
    void UpdataPos(Vector2 pos)
    {
        transform.position = pos;
    }
    public void PrepareForAlgorithm()
    {
        StartCoroutine(_PrepareForAlgorithm());

    }
    IEnumerator _PrepareForAlgorithm()
    {
        UpdatePosAnimatorInfo info = new UpdatePosAnimatorInfo(gameObject, mainCam.ScreenToWorldPoint(sidePos));
        info.block = true;
        animationBuffer.Add(info);
        while (!info.completed) yield return null;
        stack.Clear();
    }
    private void Wait(float sec, string messageText = "", bool useSetting = true)
    {
        WaitAnimatorInfo info = new WaitAnimatorInfo(gameObject, sec, useSetting);
        if (messageText != "") info.messageText = messageText;
        animationBuffer.Add(info);
    }
    int H(char ch)
    {
        if (ch == '(') return 0;
        if (ch == '[') return 1;
        if (ch == '{') return 2;
        if (ch == ')') return 3;
        if (ch == ']') return 4;
        if (ch == '}') return 5;
        return -1;
    }

    IEnumerator _BracketCheck(string s)
    {
        Debug.Log("Bracket Check ready to start, input = " + s);
        VisualizedSeqList list = Instantiate(bracketListPrefab).GetComponent<VisualizedSeqList>();
        list.UpdatePos(new Vector2(-2.5f,0));
        list.animationBuffer = animationBuffer;
        list.list.animationBuffer = animationBuffer;
        for (int i = 0; i < s.Length; i++)
        {
            VisualizedSeqElement newElement = list.Append().image.GetComponent<VisualizedSeqElement>();
            newElement.SetText(s[i].ToString());
        }
        yield return new WaitForSeconds(1);
        bool isValid = true;
        for (int i = 0; i < s.Length; i++)
        {
            StackElement topElement = stack.Top();
            StackElement newElement = Push(H(s[i]));
            list.list.array[i].Highlight(true, Palette.Pointed);
            newElement.SetText(s[i].ToString());
            newElement.bracketID = i;
            yield return new WaitForSeconds(1);
            if (H(s[i]) >= 3) {
                if (topElement == null) {
                    list.list.array[i].Highlight(true, Palette.Error);
                    isValid = false;
                    break;
                }
                else {
                    if (topElement.value + 3 != H(s[i]))
                    {
                        newElement.Highlight(true, Palette.Error);
                        topElement.Highlight(true, Palette.Error);
                        list.list.array[topElement.bracketID].Highlight(true, Palette.Error);
                        list.list.array[i].Highlight(true, Palette.Error);
                        isValid = false;
                        break;
                    }
                    else {
                        newElement.Highlight(true, Palette.Correct);
                        topElement.Highlight(true, Palette.Correct);
                        yield return new WaitForSeconds(1);
                        Pop(0.01f);
                        Pop(0.01f);
                        yield return new WaitForSeconds(1);
                    }
                }
            }
            list.list.array[i].SetColor(Palette.Normal);
        }
        if (!stack.IsEmpty()) isValid = false;
        Message message = Instantiate(messagePrefab).GetComponent<Message>();
        if (isValid) {
            message.SetText("The bracket sequence is valid.");
        }
        else {
            message.SetText("The bracket sequence is invalid!");
        }
        message.StartBreathing();
        Debug.Log("Algorithm Complete");
        yield return new WaitForSeconds(1);
        message.FadeOut();
    }
    public void BracketCheck(string s)
    {
        //StartCoroutine(_BracketCheck(s));
        
        Debug.Log("Bracket Check ready to start, input = " + s);
        list = Instantiate(bracketListPrefab).GetComponent<VisualizedSeqList>();
        list.UpdatePos(new Vector2(-2.5f,0));
        list.animationBuffer = animationBuffer;
        list.list.animationBuffer = animationBuffer;
        for (int i = 0; i < s.Length; i++)
        {
            VisualizedSeqElement newElement = list.Append().image.GetComponent<VisualizedSeqElement>();
            newElement.SetText(s[i].ToString());
        }
        Wait(1f);
        bool isValid = true;
        for (int i = 0; i < s.Length; i++)
        {
            StackElement topElement = stack.Top();
            StackElement newElement = Push(H(s[i]));
            list.list.array[i].Highlight(true, Palette.Pointed);
            newElement.SetText(s[i].ToString());
            newElement.bracketID = i;
            Wait(1f);
            if (H(s[i]) >= 3) {
                 if (topElement == null) {
                    list.list.array[i].Highlight(true, Palette.Error);
                    isValid = false;
                    break;
                }
                else {
                    if (topElement.value + 3 != H(s[i]))
                    {
                        newElement.Highlight(true, Palette.Error);
                        topElement.Highlight(true, Palette.Error);
                        list.list.array[topElement.bracketID].Highlight(true, Palette.Error);
                        list.list.array[i].Highlight(true, Palette.Error);
                        isValid = false;
                        break;
                    }
                    else {
                        newElement.Highlight(true, Palette.Correct);
                        topElement.Highlight(true, Palette.Correct);
                        list.list.array[topElement.bracketID].Highlight(true, Palette.Correct);
                        list.list.array[i].Highlight(true, Palette.Correct);
                        Wait(1f);
                        Pop(0.01f);
                        Pop(0.01f);
                        Wait(1f);
                    }
                }
            }
            else list.list.array[i].SetColor(Palette.Normal);
        }
        if (!stack.IsEmpty()) isValid = false;

        string message;
        if (isValid) {
            message = "The bracket sequence is valid.";
        }
        else {
            message = "The bracket sequence is invalid!";
        }
        Wait(-1, message);
        WaitUntilAlgorithmFinished();
    }
    void WaitUntilAlgorithmFinished()
    {
        Global.mouseMode = Global.MouseMode.AddEdge;
        StartCoroutine(_WaitUntilAlgorithmFinished());
    }
    IEnumerator _WaitUntilAlgorithmFinished()
    {
        Global.mouseMode = Global.MouseMode.AddEdge;
        while (true)
        {
            if (Global.waitingEventCount <= 0) break;
            yield return null;
        }
        list.Destroy();
        stack.Clear();
    }
}
