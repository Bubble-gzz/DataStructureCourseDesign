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
                selected[i] = true;
                selectCount++;
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
                selectCount--;
            }
            lists[i].HideBar();
            lists[i].selectable = false;
        }

        message.FadeOut();
    }
}
