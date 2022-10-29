using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MatrixEntry : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rect;
    [SerializeField]
    Vector2 anchoredPosition, localPosition, worldPosition;
    TMP_Text text;
    public Matrix matrix;
    float size0;
    void Awake()
    {
        //Debug.Log("matrixEntry Awake");
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<TMP_Text>();
        size0 = text.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        anchoredPosition = rect.anchoredPosition;
        localPosition = rect.localPosition;
        worldPosition = rect.position;
        text.fontSize = size0 * matrix.entrySize.x / matrix.defaultEntrySize.x;
    }
}
