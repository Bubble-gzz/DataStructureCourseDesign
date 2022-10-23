using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalLine : MonoBehaviour
{
    AnimationBuffer animationBuffer;
    [SerializeField]
    GameObject edgePanelPrefab;
    [SerializeField]
    Vector2 panelOffset;
    [SerializeField]
    public GameObject root;
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
    }
    void OnMouseEnter()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut));
    }
    void OnMouseExit()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack));
    }
    void OnMouseDown()
    {
        GameObject newPanel = Instantiate(edgePanelPrefab);
        newPanel.GetComponentInChildren<EdgePanel>().edge = root;
        newPanel.transform.position = (Vector2)transform.position + panelOffset;
    }
}
