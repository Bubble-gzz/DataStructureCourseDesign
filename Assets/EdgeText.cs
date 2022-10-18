using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject edgeObject;
    RectTransform rectTransform;

    // Update is called once per frame
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
       // rectTransform. = edgeObject.transform.position;
    }
}
