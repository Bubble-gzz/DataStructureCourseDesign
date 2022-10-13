using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
class AnimationBuffer : MonoBehaviour{
    [SerializeField]
    public int order;
    [SerializeField]
    public int latestOrder;
    void Update()
    {

    }
    public void Wait(float sec)
    {
        StartCoroutine(_Wait(sec));
    }
    private IEnumerator _Wait(float sec)
    {
        int order;
        latestOrder++;
        order = latestOrder;
        while (this.order != order) yield return null;
        yield return new WaitForSeconds(sec);
        this.order++;
    }
}