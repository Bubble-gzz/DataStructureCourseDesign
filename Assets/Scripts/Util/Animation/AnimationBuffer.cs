using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AnimationBuffer : MonoBehaviour{
    Queue<AnimationInfo> queue;
    [SerializeField]
    int head, tail;
    void Start()
    {
        queue = new Queue<AnimationInfo>();
        head = 0;
        tail = -1;
        StartCoroutine(Render());
    }
    IEnumerator Render()
    {
        while (true)
        {
            if (queue.Count == 0) {
                yield return null;
                continue;
            }
            AnimationInfo info = queue.Peek();
            info.Invoke();
            while (!info.completed) yield return null;
            queue.Dequeue();
            head++;
        }
    }
    public void Add(AnimationInfo info)
    {
        queue.Enqueue(info);
        tail++;
    }
}