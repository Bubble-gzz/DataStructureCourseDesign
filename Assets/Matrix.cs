using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Matrix : MonoBehaviour
{
    const int size = 100;
    RectTransform rect, maskedRect, titleRect;
    [SerializeField]
    GameObject entryPrefab, indexPrefab;
    [SerializeField]
    public float spaceH, spaceV;
    float defaultSpaceH, defaultSpaceV;
    [SerializeField]
    public int row = 0, col = 0;
    [SerializeField]
    public Vector2 entrySize;
    public Vector2 defaultEntrySize;
    [SerializeField]
    bool indexed = true;
    bool hasValue = true;
    GameObject[,] entries = new GameObject[size, size];
    public bool fixPanelSize = true;
    [SerializeField]
    public Vector2 panelSize;
    void Awake()
    {
        rect = transform.Find("Matrix").GetComponent<RectTransform>();
        maskedRect = transform.Find("MaskedMatrix").GetComponent<RectTransform>();
        titleRect = transform.Find("Matrix/Title").GetComponent<RectTransform>();
        if (indexed) titleRect.anchoredPosition = new Vector2(0, 35);
        else titleRect.anchoredPosition = new Vector2(0, 5);
        defaultEntrySize = entrySize;
        defaultSpaceH = spaceH;
        defaultSpaceV = spaceV;
    }
    void Start()
    {

    }
    void Update()
    {
    }
    void Create()
    {
        if (fixPanelSize) {
            rect.sizeDelta = panelSize;
            entrySize.x = panelSize.x * 0.95f / col;
            if (col > 1) spaceH = panelSize.x * 0.05f / (col - 1);
            entrySize.y = panelSize.y * 0.95f / row;
            if (row > 1) spaceV = panelSize.y * 0.05f / (row - 1);
        }
        else {
            entrySize = defaultEntrySize;
            spaceH = defaultSpaceH;
            spaceV = defaultSpaceV;
            rect.sizeDelta = new Vector2(spaceH * (col - 1) + entrySize.x * col, 
                                        spaceV * (row - 1) + entrySize.y * row);

        }
        maskedRect.sizeDelta = rect.sizeDelta;
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                CreateEntry(i, j);
                if (indexed) CreateIndex(i, j);
            }
    }
    public void Refresh(int newRow, int newCol)
    {
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                Destroy(entries[i, j]);
        foreach (Transform child in transform.Find("Matrix"))
            if (child.tag != "Title") Destroy(child.gameObject);
        row = newRow; col = newCol;
        Create();
    }
    public void Refresh(int newRow, int newCol, float[,] d)
    {
        Refresh(newRow, newCol);
        for (int i = 0; i < newRow; i++)
            for (int j = 0; j < newCol; j++)
                ChangeText(i, j, d[i, j] > 100000 ? "-" : d[i, j].ToString("f0"));
    }
    void CreateEntry(int i, int j)
    {
        RectTransform entryTransform = Instantiate(entryPrefab, maskedRect.transform).GetComponent<RectTransform>();
        entries[i, j] = entryTransform.gameObject;
        if (hasValue) ChangeText(i, j, "0");
        else ChangeText(i, j, "");
        MatrixEntry newEntry = entryTransform.GetComponent<MatrixEntry>();
        newEntry.matrix = this;
        entryTransform.gameObject.SetActive(true);
        newEntry.UpdateText();
        entryTransform.sizeDelta = entrySize;
        entryTransform.anchoredPosition = new Vector2(j * (spaceH + entrySize.x), -i * (spaceV + entrySize.y));
    }
    void CreateIndex(int i, int j)
    {
        if (i == 0)
        {
            RectTransform indexTransform = Instantiate(indexPrefab, rect).GetComponent<RectTransform>();
            indexTransform.gameObject.SetActive(true);
            indexTransform.anchoredPosition = new Vector2(j * spaceH + (j + 0.5f) * entrySize.x, 20);
            indexTransform.GetComponent<TMP_Text>().text = j.ToString();
        }
        if (j == 0)
        {
            RectTransform indexTransform = Instantiate(indexPrefab, rect).GetComponent<RectTransform>();
            indexTransform.gameObject.SetActive(true);
            indexTransform.anchoredPosition = new Vector2(-20, - (i * spaceV + (i + 0.5f) * entrySize.y));
            indexTransform.GetComponent<TMP_Text>().text = i.ToString();
        }
    }
    public void ChangeText(int i, int j, string newText)
    {
        entries[i, j].GetComponentInChildren<TMP_Text>().text = newText;
    }
}
