using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLayOutManager : MonoBehaviour
{
    [SerializeField]
    GameObject messagePrefab;
    [SerializeField]
    GameObject listPrefab;
    [SerializeField]
    float upperBorder, lowerBorder;
    AnimationBuffer animationBuffer;
    public int listCount, selectCount;

    List<VisualizedSeqList> lists = new List<VisualizedSeqList>();
    List<bool> selected = new List<bool>();
    VisualizedSeqList newList;
    void Awake()
    {
        listCount = selectCount = 0;
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddList()
    {
        VisualizedSeqList newList = Instantiate(listPrefab, transform).GetComponent<VisualizedSeqList>();
        newList.layoutManager = this;
        newList.selectable = false;
        lists.Add(newList);
        selected.Add(false);
        listCount++;
        ReArrange();
        this.newList = newList;
    }
    void ReArrange()
    {
        float interval = (upperBorder - lowerBorder) / (listCount + 1);
        for (int i = 0; i < lists.Count; i++)
        {
            Vector2 newPos = new Vector2(-6, upperBorder - interval * (i + 1));
            animationBuffer.Add(new UpdatePosAnimatorInfo(lists[i].gameObject, newPos));
        }
    }
    public void SelectList(VisualizedSeqList list)
    {
        for (int i = 0; i < lists.Count; i++)
            if (lists[i] == list) {
                if (selected[i] == false) selectCount++;
                selected[i] = true;
                lists[i].selected = true;
            }
    }
    public void UnSelectList(VisualizedSeqList list)
    {
        for (int i = 0; i < lists.Count; i++)
            if (lists[i] == list) {
                if (selected[i]) selectCount--;
                selected[i] = false;
                lists[i].selected = false;
            }   
    }
    public void OnClickedQuickSort()
    {
        StartCoroutine(_PrepareForQuickSort());
    }
    IEnumerator _PrepareForQuickSort()
    {
        for (int i = 0; i < lists.Count; i++)
            lists[i].selectable = true;
        Message message = Instantiate(messagePrefab).GetComponent<Message>();
        message.SetText("Please select a list to sort.");
        message.StartBreathing();
        while (selectCount != 1)
            yield return null;
        for (int i = 0; i < lists.Count; i++)
        {
            if (selected[i]) {
                lists[i].Sort();
                selected[i] = false;
                lists[i].selected = false;
                selectCount--;
            }
            lists[i].HideBar();
            lists[i].selectable = false;
        }
        Global.mouseOverSeqListBar = false;
        message.FadeOut();
    }
    public void OnClickMerge()
    {
        StartCoroutine(_PrepareForMerge());
    }
    IEnumerator _PrepareForMerge()
    {
        int mergableCount = 0;
        for (int i = 0; i < lists.Count; i++)
            if (lists[i].IsOrdered()) mergableCount++;
        Debug.Log("mergableCount : " + mergableCount);
        if (mergableCount < 2) yield break;
        Message message = Instantiate(messagePrefab).GetComponent<Message>();
        message.SetText("Please select two ordered lists to merge.");
        message.StartBreathing();
        for (int i = 0; i < lists.Count; i++)
            if (lists[i].IsOrdered()) lists[i].selectable = true;
        while (selectCount < 2)
            yield return null;
        List<VisualizedSeqList> mergeLists = new List<VisualizedSeqList>();
        for (int i = 0; i < lists.Count; i++)
        {
            if (selected[i]) {
                mergeLists.Add(lists[i]);
                selected[i] = false;
                lists[i].selected = false;
                selectCount--;
            }
            lists[i].selectable = false;
        }
        Global.mouseOverSeqListBar = false;
        message.FadeOut();
        StartCoroutine(_Merge(mergeLists[0].list, mergeLists[1].list));
    }
    private void Wait(float sec, string messageText = "", bool useSetting = true)
    {
        WaitAnimatorInfo info = new WaitAnimatorInfo(gameObject, sec, useSetting);
        if (messageText != "") info.messageText = messageText;
        animationBuffer.Add(info);
    }
    IEnumerator _Merge(SeqList A, SeqList B)
    {
        AddList();
        VisualizedSeqList C = this.newList;
        yield return new WaitForSeconds(1f);
        
        A.WakeUpPointer(A.pointer_i, new Vector2(0, 1.1f), false);
        B.WakeUpPointer(B.pointer_j, new Vector2(0, -1.1f), false);
        A.UpdatePointerPos(A.pointer_i, A.array[0].Position(), false);
        B.UpdatePointerPos(B.pointer_j, B.array[0].Position(), false);
        A.PointerAppear(A.pointer_i);
        B.PointerAppear(B.pointer_j);
        int i = 0, j = 0;
        while (i < A.Size() || j < B.Size())
        {
            SeqElement A0, B0, C0;
            if (i >= A.Size()) A0 = SeqElement.BigConst();
            else A0 = A.GetElement(i);
            if (j >= B.Size()) B0 = SeqElement.BigConst();
            else B0 = B.GetElement(j);
            if (A0 <= B0)
            {
                C0 = A0;
                C0.Highlight(true, Palette.BetterElement);
                yield return new WaitForSeconds(0.5f);
                i++;
                A.UpdatePointerPos(A.pointer_i, A.array[i].Position());
            }
            else
            {
                C0 = B0;
                C0.Highlight(true, Palette.BetterElement);
                yield return new WaitForSeconds(0.5f);
                j++;
                B.UpdatePointerPos(B.pointer_j, B.array[j].Position());
            }
            C.Append(C0.value);
            yield return new WaitForSeconds(0.5f);
            C0.SetColor(Palette.Normal);
        }

    }
}
